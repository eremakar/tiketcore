using Data.Loading.Models;
using OfficeOpenXml;

namespace Data.Loading;

public class TrainExcelService
{
    private readonly string filePath;
    private const string TargetHeaderText = "Маршрутное расписание";

    public TrainExcelService(string filePath)
    {
        this.filePath = filePath;
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }

    public List<RouteData> ReadAllSheets()
    {
        var result = new List<RouteData>();
        var fileInfo = new FileInfo(filePath);

        if (!fileInfo.Exists)
        {
            throw new FileNotFoundException($"Excel файл не найден: {filePath}");
        }

        using var package = new ExcelPackage(fileInfo);
        
        if (package.Workbook.Worksheets.Count == 0)
        {
            throw new InvalidOperationException("Excel файл не содержит вкладок");
        }

        Console.WriteLine($"Найдено вкладок: {package.Workbook.Worksheets.Count}\n");

        // Последовательно читаем все вкладки
        foreach (var worksheet in package.Workbook.Worksheets)
        {
            Console.WriteLine($"Обрабатываю вкладку: {worksheet.Name}");

            // Пропускаем скрытые вкладки
            if (worksheet.Hidden != eWorkSheetHidden.Visible)
            {
                Console.WriteLine($"  Пропущена (скрытая)\n");
                continue;
            }

            if (worksheet.Dimension == null)
            {
                Console.WriteLine($"  Пропущена (пустая)\n");
                continue;
            }

            var rowCount = worksheet.Dimension.Rows;
            var colCount = worksheet.Dimension.Columns;

            // Проверяем первую строку
            if (!IsValidRouteScheduleSheet(worksheet, colCount))
            {
                Console.WriteLine($"  Пропущена (не найдено '{TargetHeaderText}')\n");
                continue;
            }

            Console.WriteLine($"  ✓ Найдено '{TargetHeaderText}'");
            Console.WriteLine($"  Строк: {rowCount}, Столбцов: {colCount}");

            // Ищем строку с номерами поездов
            var (trainNumbers, trainNumberRow) = FindTrainNumbers(worksheet, rowCount, colCount);
            if (trainNumbers.Count != 2)
            {
                Console.WriteLine($"  ✗ Ошибка: найдено {trainNumbers.Count} поездов, ожидается ровно 2");
                Console.WriteLine($"  Пропущена\n");
                continue;
            }
            
            Console.WriteLine($"  ✓ Найдено поездов: {trainNumbers.Count} - {string.Join(", ", trainNumbers)}");

            // Ищем строку со станциями (может быть сразу после или через пустую строку)
            var (trainRoutes, trainRouteRow) = FindTrainRoutes(worksheet, trainNumberRow, rowCount, colCount);
            if (trainRoutes.Count != 2)
            {
                Console.WriteLine($"  ✗ Ошибка: найдено {trainRoutes.Count} маршрутов, ожидается ровно 2");
                Console.WriteLine($"  Пропущена\n");
                continue;
            }

            // Формируем объекты поездов
            var trains = new List<Train>();
            for (int i = 0; i < 2; i++)
            {
                var (startStation, endStation) = ParseRoute(trainRoutes[i]);
                trains.Add(new Train
                {
                    Name = trainNumbers[i],
                    StartStation = startStation,
                    EndStation = endStation
                });
                Console.WriteLine($"  ✓ Поезд {trains[i].Name}: {trains[i].StartStation} → {trains[i].EndStation}");
            }

            // Ищем строку с заголовками таблицы
            var (hasHeaders, headerRow) = FindTableHeaders(worksheet, trainRouteRow + 1, rowCount, colCount);
            if (!hasHeaders)
            {
                throw new InvalidOperationException($"Не найдена строка с заголовками таблицы на вкладке '{worksheet.Name}'");
            }
            
            Console.WriteLine($"  ✓ Найдена строка заголовков (строка {headerRow})");

            // Ищем строку первого поезда (может быть через пустые строки)
            var (foundFirstTrain, firstTrainRow) = FindFirstTrainRow(worksheet, headerRow + 1, rowCount, colCount, trains[0].Name);
            if (!foundFirstTrain)
            {
                throw new InvalidOperationException($"Не найдена строка первого поезда '№ {trains[0].Name}' на вкладке '{worksheet.Name}'");
            }
            
            Console.WriteLine($"  ✓ Найдена строка первого поезда (строка {firstTrainRow})");

            // Парсим маршрут первого поезда
            var (train1Route, secondTrainRow) = ParseTrainRoute(worksheet, firstTrainRow + 1, rowCount, colCount, trains[1].Name);
            trains[0].RouteItems = train1Route.Item1;
            train1Route.Item2.Reverse();
            trains[1].RouteItems = train1Route.Item2;
            
            Console.WriteLine($"  ✓ Распарсено станций для поезда {trains[0].Name}: {trains[0].RouteItems.Count}");
            Console.WriteLine($"  ✓ Распарсено станций для поезда {trains[1].Name}: {trains[1].RouteItems.Count}");
            Console.WriteLine($"  ✓ Найдена строка второго поезда (строка {secondTrainRow})");

            result.Add(new RouteData
            {
                Name = worksheet.Name,
                Train1 = trains[0],
                Train2 = trains[1]
            });

            Console.WriteLine($"  ✓ Добавлен маршрут: {worksheet.Name}\n");
        }

        return result;
    }

    private bool IsValidRouteScheduleSheet(ExcelWorksheet worksheet, int colCount)
    {
        // Ищем в первой строке единственную непустую ячейку с текстом "Маршрутное расписание"
        var nonEmptyCells = new List<(int col, string value)>();

        for (int col = 1; col <= colCount; col++)
        {
            var cellValue = worksheet.Cells[1, col].Value?.ToString()?.Trim();
            if (!string.IsNullOrWhiteSpace(cellValue))
            {
                nonEmptyCells.Add((col, cellValue));
            }
        }

        // Должна быть ровно одна непустая ячейка с текстом "Маршрутное расписание"
        return nonEmptyCells.Count == 1 && 
               nonEmptyCells[0].value.Equals(TargetHeaderText, StringComparison.OrdinalIgnoreCase);
    }

    private (List<string> trainNumbers, int row) FindTrainNumbers(ExcelWorksheet worksheet, int rowCount, int colCount)
    {
        var trainNumbers = new List<string>();

        // Ищем строку с "Поезд №" - может быть через одну пустую строку перед "Маршрутное расписание"
        // или сразу после него, проверяем первые несколько строк
        for (int row = 1; row <= Math.Min(5, rowCount); row++)
        {
            for (int col = 1; col <= colCount; col++)
            {
                var cellValue = worksheet.Cells[row, col].Value?.ToString()?.Trim();
                
                // Ищем ячейку с "Поезд №"
                if (!string.IsNullOrWhiteSpace(cellValue) && 
                    cellValue.Contains("Поезд", StringComparison.OrdinalIgnoreCase) && 
                    cellValue.Contains("№"))
                {
                    // Проверяем следующие ячейки в этой строке на наличие номеров поездов (пропускаем саму ячейку "Поезд №")
                    for (int nextCol = col + 1; nextCol <= colCount; nextCol++)
                    {
                        var trainNumber = worksheet.Cells[row, nextCol].Value?.ToString()?.Trim();
                        // Добавляем только если это не пустая строка и не содержит "Поезд №"
                        if (!string.IsNullOrWhiteSpace(trainNumber) && 
                            !(trainNumber.Contains("Поезд", StringComparison.OrdinalIgnoreCase) && trainNumber.Contains("№")))
                        {
                            trainNumbers.Add(trainNumber);
                        }
                    }
                    
                    // Если нашли "Поезд №", возвращаем номера и номер строки
                    return (trainNumbers, row);
                }
            }
        }

        return (trainNumbers, 0);
    }

    private (List<string> routes, int row) FindTrainRoutes(ExcelWorksheet worksheet, int trainNumberRow, int rowCount, int colCount)
    {
        var routes = new List<string>();

        if (trainNumberRow == 0)
            return (routes, 0);

        // Проверяем следующую строку и через одну (если есть пустая строка)
        for (int row = trainNumberRow + 1; row <= Math.Min(trainNumberRow + 2, rowCount); row++)
        {
            // Проверяем, есть ли непустые ячейки в этой строке
            var hasContent = false;
            for (int col = 1; col <= colCount; col++)
            {
                var cellValue = worksheet.Cells[row, col].Value?.ToString()?.Trim();
                if (!string.IsNullOrWhiteSpace(cellValue))
                {
                    hasContent = true;
                    break;
                }
            }

            // Пропускаем пустую строку
            if (!hasContent)
                continue;

            // Если нашли строку с контентом - это строка с маршрутами
            // Читаем все непустые ячейки
            for (int col = 1; col <= colCount; col++)
            {
                var cellValue = worksheet.Cells[row, col].Value?.ToString()?.Trim();
                if (!string.IsNullOrWhiteSpace(cellValue))
                {
                    routes.Add(cellValue);
                }
            }

            return (routes, row);
        }

        return (routes, 0);
    }

    private (string startStation, string endStation) ParseRoute(string route)
    {
        // Парсим маршрут в формате "Станция А - Станция Б" или "Станция А-Станция Б"
        var parts = route.Split(new[] { " - ", " — " }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        
        if (parts.Length >= 2)
        {
            return (parts[0].Trim(), parts[1].Trim());
        }
        
        // Если не удалось разделить, возвращаем весь маршрут как начальную станцию
        return (route.Trim(), string.Empty);
    }

    private (bool found, int row) FindTableHeaders(ExcelWorksheet worksheet, int startRow, int rowCount, int colCount)
    {
        // Обязательные заголовки колонок
        var requiredHeaders = new[] { "Прибы-", "Сто-", "Отправ-", "Раздельные", "Расст.", "Код", "Прибы-", "Сто-", "Отправ-" };
        // Необязательные заголовки
        var optionalHeaders = new[] { "Сл." };
        
        // Ищем строку с заголовками после строки с номерами поездов (может быть через пустые строки)
        for (int row = startRow; row <= Math.Min(startRow + 10, rowCount); row++)
        {
            var cellValues = new List<string>();
            
            // Читаем все непустые ячейки в строке
            for (int col = 1; col <= colCount; col++)
            {
                var cellValue = worksheet.Cells[row, col].Value?.ToString()?.Trim();
                if (!string.IsNullOrWhiteSpace(cellValue))
                {
                    cellValues.Add(cellValue);
                }
            }

            // Пропускаем пустые строки
            if (cellValues.Count == 0)
                continue;

            // Пропускаем строки с "Общ.вр.в пути"
            if (cellValues.Any(cv => cv.Contains("Общ.вр.в пути", StringComparison.OrdinalIgnoreCase)))
                continue;

            // Проверяем, содержит ли строка все обязательные заголовки
            if (cellValues.Count >= requiredHeaders.Length)
            {
                bool allFound = true;
                foreach (var expectedHeader in requiredHeaders)
                {
                    if (!cellValues.Any(cv => cv.Contains(expectedHeader, StringComparison.OrdinalIgnoreCase)))
                    {
                        allFound = false;
                        break;
                    }
                }

                if (allFound)
                {
                    return (true, row);
                }
            }
        }

        return (false, 0);
    }

    private (bool found, int row) FindFirstTrainRow(ExcelWorksheet worksheet, int startRow, int rowCount, int colCount, string expectedTrainNumber)
    {
        // Ищем строку с "Поезд №" и номером первого поезда (может быть через пустые строки)
        for (int row = startRow; row <= Math.Min(startRow + 10, rowCount); row++)
        {
            var cellValues = new List<string>();
            
            // Читаем все непустые ячейки в строке
            for (int col = 1; col <= colCount; col++)
            {
                var cellValue = worksheet.Cells[row, col].Value?.ToString()?.Trim();
                if (!string.IsNullOrWhiteSpace(cellValue))
                {
                    cellValues.Add(cellValue);
                }
            }

            // Пропускаем пустые строки
            if (cellValues.Count == 0)
                continue;

            // Проверяем, содержит ли строка "Поезд №" и номер поезда
            bool hasTrainLabel = cellValues.Any(cv => 
                cv.Contains("Поезд", StringComparison.OrdinalIgnoreCase) && 
                cv.Contains("№"));
            
            bool hasTrainNumber = cellValues.Any(cv => 
                cv.Trim().Equals(expectedTrainNumber, StringComparison.OrdinalIgnoreCase) ||
                cv.Trim().TrimStart('0').Equals(expectedTrainNumber.TrimStart('0'), StringComparison.OrdinalIgnoreCase));

            if (hasTrainLabel && hasTrainNumber)
            {
                return (true, row);
            }
        }

        return (false, 0);
    }

    private ((List<RouteItem>, List<RouteItem>) routes, int secondTrainRow) ParseTrainRoute(ExcelWorksheet worksheet, int startRow, int rowCount, int colCount, string secondTrainNumber)
    {
        var train1Items = new List<RouteItem>();
        var train2Items = new List<RouteItem>();
        int secondTrainRow = 0;

        // Парсим строки до второго поезда
        for (int row = startRow; row <= rowCount; row++)
        {
            // Проверяем, это строка второго поезда
            var cellValues = new List<string>();
            for (int col = 1; col <= colCount; col++)
            {
                var cellValue = worksheet.Cells[row, col].Value?.ToString()?.Trim();
                if (!string.IsNullOrWhiteSpace(cellValue))
                {
                    cellValues.Add(cellValue);
                }
            }

            // Пропускаем пустые строки
            if (cellValues.Count == 0)
                continue;

            // Проверяем на второй поезд
            bool hasTrainLabel = cellValues.Any(cv => 
                cv.Contains("Поезд", StringComparison.OrdinalIgnoreCase) && 
                cv.Contains("№"));
            
            bool hasSecondTrainNumber = cellValues.Any(cv => 
                cv.Trim().Equals(secondTrainNumber, StringComparison.OrdinalIgnoreCase) ||
                cv.Trim().TrimStart('0').Equals(secondTrainNumber.TrimStart('0'), StringComparison.OrdinalIgnoreCase));

            if (hasTrainLabel && hasSecondTrainNumber)
            {
                secondTrainRow = row;
                break;
            }

            // Пропускаем строки с двумя номерами поездов (№ 004 и т.п.)
            int trainNumberCount = cellValues.Count(cv => 
                cv.StartsWith("№") || 
                (cv.Contains("№") && cv.Length <= 10));
            
            if (trainNumberCount >= 2)
                continue;

            // Парсим данные маршрута
            // Структура: Прибы-(1) Сто-(1) Отправ-(1) Раздельные Расст. Код Прибы-(2) Сто-(2) Отправ-(2)
            // Позиции колонок: 1, 2, 3, 4, 5, 6, 7, 8, 9
            var arrival1 = worksheet.Cells[row, 1].Value?.ToString()?.Trim();
            var stop1 = worksheet.Cells[row, 2].Value?.ToString()?.Trim();
            var departure1 = worksheet.Cells[row, 3].Value?.ToString()?.Trim();
            var stationName = worksheet.Cells[row, 4].Value?.ToString()?.Trim();
            var distance = worksheet.Cells[row, 5].Value?.ToString()?.Trim();
            var stationCode = worksheet.Cells[row, 6].Value?.ToString()?.Trim();
            var arrival2 = worksheet.Cells[row, 7].Value?.ToString()?.Trim();
            var stop2 = worksheet.Cells[row, 8].Value?.ToString()?.Trim();
            var departure2 = worksheet.Cells[row, 9].Value?.ToString()?.Trim();

            // Добавляем только если есть название станции
            if (!string.IsNullOrWhiteSpace(stationName))
            {
                train1Items.Add(new RouteItem
                {
                    Arrival = arrival1,
                    Stop = stop1,
                    Departure = departure1,
                    StationName = stationName,
                    Distance = distance,
                    StationCode = stationCode
                });

                train2Items.Add(new RouteItem
                {
                    Arrival = arrival2,
                    Stop = stop2,
                    Departure = departure2,
                    StationName = stationName,
                    Distance = distance,
                    StationCode = stationCode
                });
            }
        }

        return ((train1Items, train2Items), secondTrainRow);
    }

    public void PrintData(List<RouteData> routes, int maxRowsPerSheet = 10)
    {
        Console.WriteLine($"\n=== ИТОГО ===");
        Console.WriteLine($"Обработано маршрутов: {routes.Count}\n");

        foreach (var route in routes)
        {
            Console.WriteLine($"\nМаршрут: {route.Name}");
            
            Console.WriteLine($"  Поезд 1 - {route.Train1.Name}: {route.Train1.StartStation} → {route.Train1.EndStation}");
            Console.WriteLine($"    Станций в маршруте: {route.Train1.RouteItems.Count}");
            if (route.Train1.RouteItems.Count > 0)
            {
                Console.WriteLine($"    Первая: {route.Train1.RouteItems.First().StationName}");
                Console.WriteLine($"    Последняя: {route.Train1.RouteItems.Last().StationName}");
            }
            
            Console.WriteLine($"  Поезд 2 - {route.Train2.Name}: {route.Train2.StartStation} → {route.Train2.EndStation}");
            Console.WriteLine($"    Станций в маршруте: {route.Train2.RouteItems.Count}");
            if (route.Train2.RouteItems.Count > 0)
            {
                Console.WriteLine($"    Первая: {route.Train2.RouteItems.First().StationName}");
                Console.WriteLine($"    Последняя: {route.Train2.RouteItems.Last().StationName}");
            }
        }
    }
}