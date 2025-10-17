using Data.Loading.Models;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Data.Loading;

public class TrainWordService
{
    private readonly string filePath;

    public TrainWordService(string filePath)
    {
        this.filePath = filePath;
    }

    public RouteData? ReadDocument()
    {
        var fileInfo = new FileInfo(filePath);

        if (!fileInfo.Exists)
        {
            throw new FileNotFoundException($"Word файл не найден: {filePath}");
        }

        // Проверяем расширение - работаем только с .docx
        if (fileInfo.Extension.ToLower() != ".docx")
        {
            Console.WriteLine($"  Пропущен (поддерживаются только .docx файлы): {fileInfo.Name}");
            throw new Exception("Unsupported file format");
        }

        try
        {
            using var wordDocument = WordprocessingDocument.Open(filePath, false);
            var body = wordDocument.MainDocumentPart?.Document?.Body;

            if (body == null)
            {
                Console.WriteLine($"  Пропущен (пустой документ): {fileInfo.Name}");
                throw new Exception("Empty document");
            }

            // Берем первые 2 непустых параграфа и объединяем их
            var nonEmptyParagraphs = body.Descendants<Paragraph>()
                .Where(p => !string.IsNullOrWhiteSpace(p.InnerText))
                .Take(2)
                .Select(p => p.InnerText?.Trim() ?? string.Empty)
                .ToList();
            
            if (nonEmptyParagraphs.Count == 0)
            {
                Console.WriteLine($"  ✗ Ошибка: не найдено непустых параграфов: {fileInfo.Name}");
                throw new Exception("No non-empty paragraphs found");
            }

            var headerText = string.Join(" ", nonEmptyParagraphs).Replace("  ", " ").Trim();
            
            if (!headerText.StartsWith("Схема состава", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"  ✗ Ошибка: документ не начинается с 'Схема состава': {fileInfo.Name}");
                Console.WriteLine($"    Найдено: '{headerText.Substring(0, Math.Min(50, headerText.Length))}'");
                throw new Exception("Invalid document format");
            }
            Console.WriteLine($"  ✓ Найдено 'Схема состава'");

            // Парсим данные из заголовка
            var (train1Number, train2Number, startStation, endStation) = ParseSchemaHeader(headerText);
            Console.WriteLine($"  ✓ Поезда: {train1Number} / {train2Number}");
            Console.WriteLine($"  ✓ Маршрут: {startStation} → {endStation}");

            // Извлекаем первую таблицу
            var firstTable = body.Elements<Table>().FirstOrDefault();
            if (firstTable == null)
            {
                Console.WriteLine($"  ✗ Ошибка: нет таблиц в документе: {fileInfo.Name}");
                throw new Exception("No tables found in document");
            }

            // Проверяем что это таблица со схемой состава
            if (!IsWagonSchemaTable(firstTable))
            {
                Console.WriteLine($"  ✗ Ошибка: первая таблица не содержит обязательные колонки схемы состава: {fileInfo.Name}");
                throw new Exception("First table is not a wagon schema table");
            }

            Console.WriteLine($"  ✓ Найдена таблица со схемой состава");

            // Парсим вагоны из таблицы
            var wagons = ParseWagonsFromTable(firstTable);
            Console.WriteLine($"  ✓ Распарсено вагонов: {wagons.Count}");

            if (wagons.Count == 0)
            {
                Console.WriteLine($"  ✗ Ошибка: не удалось распарсить вагоны из таблицы: {fileInfo.Name}");
                throw new Exception("No wagons parsed from table");
            }

            return new RouteData
            {
                Name = Path.GetFileNameWithoutExtension(filePath),
                Train1 = new Train 
                { 
                    Name = train1Number, 
                    StartStation = startStation, 
                    EndStation = endStation,
                    Wagons = wagons
                },
                Train2 = new Train 
                { 
                    Name = train2Number, 
                    StartStation = endStation, 
                    EndStation = startStation,
                    Wagons = wagons
                }
            };
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"  ✗ Ошибка при чтении Word документа {fileInfo.Name}: {ex.Message}");
            return null;
        }
    }

    private List<TrainWagon> ParseWagonsFromTable(Table table)
    {
        var wagons = new List<TrainWagon>();
        var rows = table.Elements<TableRow>().ToList();
        
        if (rows.Count < 3)
            return wagons;

        // Определяем индексы колонок из первых двух строк заголовка
        var (numberCol, typeCol, seatClassColumns) = GetColumnIndices(rows[0], rows[1]);

        // Обрабатываем все строки данных (начиная с третьей)
        for (int rowIndex = 2; rowIndex < rows.Count; rowIndex++)
        {
            var dataRow = rows[rowIndex];
            var cells = dataRow.Elements<TableCell>().ToList();

            // Извлекаем текстовые линии из каждой ячейки (каждый параграф = новая строка)
            var columnLines = new List<string[]>();
            for (int cellIndex = 0; cellIndex < cells.Count; cellIndex++)
            {
                // Проверяем, является ли эта колонка колонкой с количеством мест
                bool isSeatColumn = seatClassColumns.Any(sc => sc.colIndex == cellIndex);
                
                var lines = GetCellLines(cells[cellIndex], splitBySpaces: isSeatColumn);
                columnLines.Add(lines);
            }

            // Находим максимальное количество строк в текущей физической строке таблицы
            int maxLines = columnLines.Any() ? columnLines.Max(c => c.Length) : 0;

            // Парсим каждую логическую строку данных внутри ячеек
            for (int lineIndex = 0; lineIndex < maxLines; lineIndex++)
            {
                // Получаем значение из каждой колонки для текущей строки
                var numberText = numberCol >= 0 && numberCol < columnLines.Count && lineIndex < columnLines[numberCol].Length
                    ? columnLines[numberCol][lineIndex] : "";
                var typeText = typeCol >= 0 && typeCol < columnLines.Count && lineIndex < columnLines[typeCol].Length
                    ? columnLines[typeCol][lineIndex] : "";

                // Пропускаем строки с "ИТОГО"
                if (numberText.Contains("ИТОГО", StringComparison.OrdinalIgnoreCase) ||
                    numberText.Contains("Итого", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                // Пропускаем пустые строки
                if (string.IsNullOrWhiteSpace(numberText) && string.IsNullOrWhiteSpace(typeText))
                    continue;

                var wagon = new TrainWagon
                {
                    Number = numberText,
                    Type = typeText,
                    SeatCounts = new List<SeatCount>()
                };

                // Извлекаем количество мест по классам
                foreach (var (className, colIndex) in seatClassColumns)
                {
                    if (colIndex >= 0 && colIndex < columnLines.Count && lineIndex < columnLines[colIndex].Length)
                    {
                        var countText = columnLines[colIndex][lineIndex];
                        if (!string.IsNullOrWhiteSpace(countText))
                        {
                            wagon.SeatCounts.Add(new SeatCount
                            {
                                Name = className,
                                Count = countText
                            });
                        }
                    }
                }

                // Добавляем вагон только если есть номер или тип
                if (!string.IsNullOrWhiteSpace(wagon.Number) || !string.IsNullOrWhiteSpace(wagon.Type))
                {
                    wagons.Add(wagon);
                }
            }
        }

        return wagons;
    }

    private (int numberCol, int typeCol, List<(string className, int colIndex)> seatClassColumns) GetColumnIndices(TableRow headerRow1, TableRow headerRow2)
    {
        var cells1 = headerRow1.Elements<TableCell>().ToList();
        var cells2 = headerRow2.Elements<TableCell>().ToList();
        
        int numberCol = -1;
        int typeCol = -1;
        var seatClassColumns = new List<(string, int)>();

        // Находим колонку "Порядковый № вагона"
        for (int i = 0; i < cells1.Count; i++)
        {
            var cellText = NormalizeText(GetCellText(cells1[i]));
            if (cellText.Contains("порядк") || cellText.Contains("вагон"))
            {
                numberCol = i;
                break;
            }
        }

        // Находим колонку "Тип вагона"
        for (int i = 0; i < cells1.Count; i++)
        {
            var cellText = NormalizeText(GetCellText(cells1[i]));
            if (cellText.Contains("тип") || cellText.Contains("категор") || cellText.Contains("род"))
            {
                typeCol = i;
                break;
            }
        }

        // Извлекаем классы вагонов из второй строки заголовка
        for (int i = 0; i < cells2.Count; i++)
        {
            var cellText = GetCellText(cells2[i]).Trim();
            var normalized = NormalizeText(cellText);
            
            // Пропускаем пустые ячейки
            if (string.IsNullOrWhiteSpace(cellText))
                continue;

            // Определяем тип класса
            string className = "";
            if (normalized == "св") className = "СВ";
            else if (normalized.Contains("купе") || normalized == "к") className = "Купе";
            else if (normalized.Contains("плац") || normalized == "пл") className = "Плацкарт";
            else if (normalized.Contains("общ") || normalized == "о") className = "Общий";
            else if (normalized.Contains("турист")) className = "Турист";
            else if (normalized.Contains("сидяч")) className = "Сидячий";

            if (!string.IsNullOrWhiteSpace(className))
            {
                seatClassColumns.Add((className, i));
            }
        }

        return (numberCol, typeCol, seatClassColumns);
    }

    private bool IsWagonSchemaTable(Table table)
    {
        var rows = table.Elements<TableRow>().Take(3).ToList();
        if (rows.Count < 2)
            return false;

        // Проверяем первую строку заголовка
        var firstRowCells = rows[0].Elements<TableCell>()
            .Select(c => NormalizeText(GetCellText(c)))
            .ToList();
        
        // Ключевые слова для каждой обязательной колонки
        var requiredKeywords = new[]
        {
            new[] { "порядк", "вагон", "" },                    // Порядковый
            new[] { "типваг", "типвагон", "категориявагон", "род" },        // Тип вагона
            new[] { "пунктобращ", "пунктыобращ" }, // Пункты обращения
            new[] { "числомест", "числомст" },     // Число мест
            new[] { "количсостав", "колвосостав", "количествосоставов", "дорога" } // Количество составов
        };
        
        var firstRowMatch = requiredKeywords.Count(keywords =>
            keywords.Any(keyword => firstRowCells.Any(cell => cell.Contains(keyword)))
        );

        if (firstRowMatch < 4)
            return false;

        // Проверяем вторую строку заголовка (подколонки для "Число мест")
        var secondRowCells = rows[1].Elements<TableCell>()
            .Select(c => NormalizeText(GetCellText(c)))
            .ToList();
        
        // Разные варианты классов вагонов (ключевые слова + точные совпадения для коротких)
        var classKeywords = new[]
        {
            new[] { "св" },                        // СВ
            new[] { "купе" },                      // Купе, Купе-турист
            new[] { "плац", "плацкарт" },          // Плац, Плацкарт
            new[] { "общ" },                       // Общий
            new[] { "турист", "тур" },             // Турист, Турист сидячие
            new[] { "сидяч", "сидячие" }           // Сидячие
        };
        
        var exactMatchKeywords = new[] { "к", "пл", "о" }; // Короткие сокращения - только точное совпадение
        
        var secondRowMatch = classKeywords.Count(keywords =>
            keywords.Any(keyword => secondRowCells.Any(cell => cell.Contains(keyword)))
        );
        
        secondRowMatch += exactMatchKeywords.Count(keyword =>
            secondRowCells.Any(cell => cell == keyword)
        );

        // Должно быть хотя бы 2 из 6 подколонок (более гибкая проверка)
        return secondRowMatch >= 2;
    }

    private (string train1, string train2, string startStation, string endStation) ParseSchemaHeader(string headerText)
    {
        // Извлекаем номера поездов (паттерн: №101/102, №606Ц/605Ц, №13(К9795)/14(К9796) и т.д.)
        var trainNumbersMatch = System.Text.RegularExpressions.Regex.Match(headerText, @"№\s*(\d+[А-ЯA-Z]*)(?:\([А-ЯA-Z]?\d+\))?\s*[/-]\s*(\d+[А-ЯA-Z]*)(?:\([А-ЯA-Z]?\d+\))?");
        if (!trainNumbersMatch.Success)
        {
            throw new Exception($"Не найдены номера поездов в заголовке: {headerText}");
        }
        
        var train1 = trainNumbersMatch.Groups[1].Value;
        var train2 = trainNumbersMatch.Groups[2].Value;

        // Извлекаем маршрут (приоритет: "в сообщении" или "сообщении", затем в кавычках)
        var routeInMessageMatch = System.Text.RegularExpressions.Regex.Match(headerText, @"(?:в\s+)?сообщении\s+(.+?)(?:\s+на\s+график|\.|$)");
        var routeInQuotesMatch = System.Text.RegularExpressions.Regex.Match(headerText, @"[\u00AB\u201C](.*?)[\u00BB\u201D]");
        
        string routeText;
        if (routeInMessageMatch.Success)
        {
            routeText = routeInMessageMatch.Groups[1].Value.Trim();
        }
        else if (routeInQuotesMatch.Success)
        {
            routeText = routeInQuotesMatch.Groups[1].Value.Trim();
        }
        else
        {
            throw new Exception($"Не найден маршрут в заголовке: {headerText}");
        }
        
        // Удаляем кавычки из текста маршрута
        routeText = routeText.Replace("\u00AB", "").Replace("\u00BB", "")
                             .Replace("\u201C", "").Replace("\u201D", "")
                             .Trim();
        
        // Парсим маршрут с учетом правила "имя-цифра" = одна станция
        var (startStation, endStation) = ParseRouteWithNumberedStations(routeText);
        
        if (string.IsNullOrWhiteSpace(startStation) || string.IsNullOrWhiteSpace(endStation))
        {
            throw new Exception($"Не удалось распарсить маршрут: {routeText}");
        }

        return (train1, train2, startStation, endStation);
    }

    private (string startStation, string endStation) ParseRouteWithNumberedStations(string route)
    {
        // Нормализуем все виды тире и дефисов
        route = route.Replace('\u2013', '-') // – длинное тире
                     .Replace('\u2014', '-') // — еще длиннее тире
                     .Replace('\u2212', '-'); // − минус
        
        // Разделяем по дефису, но склеиваем обратно "имя-цифра"
        var parts = route.Split('-');
        var stations = new List<string>();
        
        for (int i = 0; i < parts.Length; i++)
        {
            var part = parts[i].Trim();
            
            // Если следующая часть - это цифра, склеиваем с текущей
            if (i + 1 < parts.Length && System.Text.RegularExpressions.Regex.IsMatch(parts[i + 1].Trim(), @"^\d+$"))
            {
                stations.Add($"{part}-{parts[i + 1].Trim()}");
                i++; // Пропускаем следующую часть
            }
            else if (!string.IsNullOrWhiteSpace(part) && !System.Text.RegularExpressions.Regex.IsMatch(part, @"^\d+$"))
            {
                stations.Add(part);
            }
        }

        if (stations.Count < 2)
        {
            throw new Exception($"Недостаточно станций в маршруте: {route}");
        }

        return (stations[0], stations[^1]);
    }

    private RouteData? ParseTable(Table table, string routeName)
    {
        var rows = table.Elements<TableRow>().ToList();
        if (rows.Count < 5)
        {
            Console.WriteLine($"  Пропущен (слишком мало строк в таблице)");
            return null;
        }

        // Ищем строку с номерами поездов
        var (trainNumbers, trainNumberRow) = FindTrainNumbers(rows);
        if (trainNumbers.Count != 2)
        {
            Console.WriteLine($"  ✗ Ошибка: найдено {trainNumbers.Count} поездов, ожидается ровно 2");
            return null;
        }

        Console.WriteLine($"  ✓ Найдено поездов: {trainNumbers.Count} - {string.Join(", ", trainNumbers)}");

        // Ищем строку с маршрутами
        var (trainRoutes, trainRouteRow) = FindTrainRoutes(rows, trainNumberRow);
        if (trainRoutes.Count != 2)
        {
            Console.WriteLine($"  ✗ Ошибка: найдено {trainRoutes.Count} маршрутов, ожидается ровно 2");
            return null;
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
        var headerRow = FindTableHeaders(rows, trainRouteRow);
        if (headerRow == -1)
        {
            Console.WriteLine($"  ✗ Ошибка: не найдена строка с заголовками таблицы");
            return null;
        }

        Console.WriteLine($"  ✓ Найдена строка заголовков (строка {headerRow})");

        // Ищем строку первого поезда
        var firstTrainRow = FindFirstTrainRow(rows, headerRow, trains[0].Name);
        if (firstTrainRow == -1)
        {
            Console.WriteLine($"  ✗ Ошибка: не найдена строка первого поезда '№ {trains[0].Name}'");
            return null;
        }

        Console.WriteLine($"  ✓ Найдена строка первого поезда (строка {firstTrainRow})");

        // Парсим маршрут
        var (train1Route, train2Route, secondTrainRow) = ParseTrainRoute(rows, firstTrainRow + 1, trains[1].Name);
        trains[0].RouteItems = train1Route;
        train2Route.Reverse();
        trains[1].RouteItems = train2Route;

        Console.WriteLine($"  ✓ Распарсено станций для поезда {trains[0].Name}: {trains[0].RouteItems.Count}");
        Console.WriteLine($"  ✓ Распарсено станций для поезда {trains[1].Name}: {trains[1].RouteItems.Count}");
        Console.WriteLine($"  ✓ Найдена строка второго поезда (строка {secondTrainRow})");

        return new RouteData
        {
            Name = routeName,
            Train1 = trains[0],
            Train2 = trains[1]
        };
    }

    private (List<string> trainNumbers, int row) FindTrainNumbers(List<TableRow> rows)
    {
        var trainNumbers = new List<string>();

        for (int rowIndex = 0; rowIndex < Math.Min(10, rows.Count); rowIndex++)
        {
            var cells = rows[rowIndex].Elements<TableCell>().ToList();
            
            for (int cellIndex = 0; cellIndex < cells.Count; cellIndex++)
            {
                var cellText = GetCellText(cells[cellIndex]);
                
                if (!string.IsNullOrWhiteSpace(cellText) &&
                    cellText.Contains("Поезд", StringComparison.OrdinalIgnoreCase) &&
                    cellText.Contains("№"))
                {
                    // Проверяем следующие ячейки на наличие номеров поездов
                    for (int nextCell = cellIndex + 1; nextCell < cells.Count; nextCell++)
                    {
                        var trainNumber = GetCellText(cells[nextCell]).Trim();
                        if (!string.IsNullOrWhiteSpace(trainNumber) &&
                            !(trainNumber.Contains("Поезд", StringComparison.OrdinalIgnoreCase) && trainNumber.Contains("№")))
                        {
                            trainNumbers.Add(trainNumber);
                        }
                    }

                    return (trainNumbers, rowIndex);
                }
            }
        }

        return (trainNumbers, -1);
    }

    private (List<string> routes, int row) FindTrainRoutes(List<TableRow> rows, int trainNumberRow)
    {
        var routes = new List<string>();

        if (trainNumberRow == -1)
            return (routes, -1);

        // Проверяем следующие несколько строк
        for (int rowIndex = trainNumberRow + 1; rowIndex < Math.Min(trainNumberRow + 3, rows.Count); rowIndex++)
        {
            var cells = rows[rowIndex].Elements<TableCell>().ToList();
            var hasContent = false;

            foreach (var cell in cells)
            {
                var cellText = GetCellText(cell).Trim();
                if (!string.IsNullOrWhiteSpace(cellText))
                {
                    hasContent = true;
                    routes.Add(cellText);
                }
            }

            if (hasContent && routes.Count >= 2)
            {
                return (routes.Take(2).ToList(), rowIndex);
            }

            if (hasContent)
                routes.Clear();
        }

        return (routes, -1);
    }

    private (string startStation, string endStation) ParseRoute(string route)
    {
        var parts = route.Split(new[] { " - ", " — ", "-" }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length >= 2)
        {
            return (parts[0].Trim(), parts[^1].Trim());
        }

        return (route.Trim(), string.Empty);
    }

    private int FindTableHeaders(List<TableRow> rows, int startRow)
    {
        var requiredHeaders = new[] { "Прибы", "Сто", "Отправ", "Раздельные", "Расст", "Код" };

        for (int rowIndex = startRow + 1; rowIndex < Math.Min(startRow + 10, rows.Count); rowIndex++)
        {
            var cells = rows[rowIndex].Elements<TableCell>().ToList();
            var cellTexts = cells.Select(c => GetCellText(c).Trim()).Where(t => !string.IsNullOrWhiteSpace(t)).ToList();

            if (cellTexts.Count == 0)
                continue;

            // Проверяем наличие всех обязательных заголовков
            bool allFound = requiredHeaders.All(header =>
                cellTexts.Any(ct => ct.Contains(header, StringComparison.OrdinalIgnoreCase)));

            if (allFound)
            {
                return rowIndex;
            }
        }

        return -1;
    }

    private int FindFirstTrainRow(List<TableRow> rows, int startRow, string expectedTrainNumber)
    {
        for (int rowIndex = startRow; rowIndex < Math.Min(startRow + 10, rows.Count); rowIndex++)
        {
            var cells = rows[rowIndex].Elements<TableCell>().ToList();
            var cellTexts = cells.Select(c => GetCellText(c).Trim()).Where(t => !string.IsNullOrWhiteSpace(t)).ToList();

            if (cellTexts.Count == 0)
                continue;

            bool hasTrainLabel = cellTexts.Any(ct =>
                ct.Contains("Поезд", StringComparison.OrdinalIgnoreCase) &&
                ct.Contains("№"));

            bool hasTrainNumber = cellTexts.Any(ct =>
                ct.Trim().Equals(expectedTrainNumber, StringComparison.OrdinalIgnoreCase) ||
                ct.Trim().TrimStart('0').Equals(expectedTrainNumber.TrimStart('0'), StringComparison.OrdinalIgnoreCase));

            if (hasTrainLabel && hasTrainNumber)
            {
                return rowIndex;
            }
        }

        return -1;
    }

    private (List<RouteItem> train1Items, List<RouteItem> train2Items, int secondTrainRow) ParseTrainRoute(
        List<TableRow> rows, int startRow, string secondTrainNumber)
    {
        var train1Items = new List<RouteItem>();
        var train2Items = new List<RouteItem>();
        int secondTrainRow = -1;

        for (int rowIndex = startRow; rowIndex < rows.Count; rowIndex++)
        {
            var cells = rows[rowIndex].Elements<TableCell>().ToList();
            var cellTexts = cells.Select(c => GetCellText(c).Trim()).Where(t => !string.IsNullOrWhiteSpace(t)).ToList();

            if (cellTexts.Count == 0)
                continue;

            // Проверяем на второй поезд
            bool hasTrainLabel = cellTexts.Any(ct =>
                ct.Contains("Поезд", StringComparison.OrdinalIgnoreCase) &&
                ct.Contains("№"));

            bool hasSecondTrainNumber = cellTexts.Any(ct =>
                ct.Trim().Equals(secondTrainNumber, StringComparison.OrdinalIgnoreCase) ||
                ct.Trim().TrimStart('0').Equals(secondTrainNumber.TrimStart('0'), StringComparison.OrdinalIgnoreCase));

            if (hasTrainLabel && hasSecondTrainNumber)
            {
                secondTrainRow = rowIndex;
                break;
            }

            // Парсим данные маршрута
            if (cells.Count >= 9)
            {
                var arrival1 = GetCellText(cells[0]).Trim();
                var stop1 = GetCellText(cells[1]).Trim();
                var departure1 = GetCellText(cells[2]).Trim();
                var stationName = GetCellText(cells[3]).Trim();
                var distance = GetCellText(cells[4]).Trim();
                var stationCode = GetCellText(cells[5]).Trim();
                var arrival2 = GetCellText(cells[6]).Trim();
                var stop2 = GetCellText(cells[7]).Trim();
                var departure2 = GetCellText(cells[8]).Trim();

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
        }

        return (train1Items, train2Items, secondTrainRow);
    }

    private string GetCellText(TableCell? cell)
    {
        if (cell == null)
            return string.Empty;

        var text = cell.InnerText ?? string.Empty;
        return text.Trim();
    }

    private string[] GetCellLines(TableCell? cell, bool splitBySpaces = false)
    {
        if (cell == null)
            return Array.Empty<string>();

        // Каждый параграф в ячейке = отдельная строка
        var paragraphs = cell.Elements<Paragraph>();
        var lines = paragraphs
            .Select(p => p.InnerText?.Trim() ?? string.Empty)
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .ToList();

        // Если нужно разделять по пробелам (для колонок с количеством мест)
        if (splitBySpaces)
        {
            var expandedLines = new List<string>();
            foreach (var line in lines)
            {
                // Разбиваем по пробелам и добавляем каждое значение отдельно
                var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                expandedLines.AddRange(parts.Select(p => p.Trim()));
            }
            return expandedLines.ToArray();
        }

        return lines.ToArray();
    }

    private string NormalizeText(string text)
    {
        // Убираем все пробелы, дефисы, переносы строк, спецсимволы и приводим к нижнему регистру
        return text.Replace(" ", "")
                   .Replace("-", "")
                   .Replace("\n", "")
                   .Replace("\r", "")
                   .Replace("\t", "")
                   .Replace("№", "")
                   .Replace(";", "")
                   .Replace(",", "")
                   .Replace(".", "")
                   .ToLower();
    }

    public static void PrintData(List<RouteData> routes)
    {
        Console.WriteLine($"\n=== ИТОГО ===");
        Console.WriteLine($"Обработано маршрутов из Word: {routes.Count}\n");

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
            Console.WriteLine($"    Вагонов в составе: {route.Train1.Wagons.Count}");

            Console.WriteLine($"  Поезд 2 - {route.Train2.Name}: {route.Train2.StartStation} → {route.Train2.EndStation}");
            Console.WriteLine($"    Станций в маршруте: {route.Train2.RouteItems.Count}");
            if (route.Train2.RouteItems.Count > 0)
            {
                Console.WriteLine($"    Первая: {route.Train2.RouteItems.First().StationName}");
                Console.WriteLine($"    Последняя: {route.Train2.RouteItems.Last().StationName}");
            }
            Console.WriteLine($"    Вагонов в составе: {route.Train2.Wagons.Count}");
        }
    }
}

