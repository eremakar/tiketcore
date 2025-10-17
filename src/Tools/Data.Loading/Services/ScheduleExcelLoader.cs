using Data.Loading.Models;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Globalization;
using Ticketing.Data.TicketDb.DatabaseContext;

namespace Data.Loading.Services;

/// <summary>
/// Сервис для загрузки расписания поездов из Excel файла
/// 
/// Формат Excel файла:
/// - Номер поезда (например, 004) в ячейке A
/// - Заголовки столбцов: Код станции, Уровень станции, Расстояние, Время приб., Время отпр., Дата отпр., Город, Код дороги, Код линии, Поезд отпр., Признаки станции
/// - Данные станций
/// 
/// Пример использования:
/// <code>
/// var loader = new ScheduleExcelLoader(dbContext);
/// var trains = await loader.LoadScheduleAsync("Расписания.xlsx");
/// </code>
/// </summary>
public class ScheduleExcelLoader
{
    private readonly TicketDbContext dbContext;

    public ScheduleExcelLoader(TicketDbContext dbContext)
    {
        this.dbContext = dbContext;
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }

    /// <summary>
    /// Загружает расписание из Excel файла
    /// </summary>
    public async Task<List<Train>> LoadScheduleAsync(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Файл не найден: {filePath}");
        }

        Console.WriteLine($"\n=== ЗАГРУЗКА РАСПИСАНИЯ ИЗ {Path.GetFileName(filePath)} ===");

        var trains = new List<Train>();

        using var package = new ExcelPackage(new FileInfo(filePath));
        var worksheet = package.Workbook.Worksheets[0]; // Первый лист

        if (worksheet == null)
        {
            throw new InvalidOperationException("Первый лист не найден в Excel файле");
        }

        Console.WriteLine($"Обработка листа: {worksheet.Name}");

        var currentRow = 2; // Начинаем со строки 2
        var maxRow = worksheet.Dimension?.End.Row ?? 0;

        while (currentRow <= maxRow)
        {
            // Ищем строку с номером поезда в ячейке B (колонка 2), C должна быть пустой
            var cellValue = worksheet.Cells[currentRow, 2].Text?.Trim(); // Колонка B
            var cellCValue = worksheet.Cells[currentRow, 3].Text?.Trim(); // Колонка C

            if (!string.IsNullOrWhiteSpace(cellValue) && string.IsNullOrWhiteSpace(cellCValue))
            {
                // Это похоже на номер поезда
                var trainNumber = cellValue;
                Console.WriteLine($"\nНайден поезд: {trainNumber}");

                // Следующая строка должна быть заголовками
                currentRow++;
                if (currentRow > maxRow)
                    break;

                // Проверяем, что это заголовки
                var header = worksheet.Cells[currentRow, 2].Text?.Trim(); // Колонка B
                if (header != "Код станции")
                {
                    Console.WriteLine($"Предупреждение: Ожидались заголовки, но найдено '{header}' на строке {currentRow}");
                }

                // Пропускаем заголовки
                currentRow++;
                
                // Пропускаем пустую строку после заголовков
                currentRow++;

                // Читаем станции поезда
                var routeItems = new List<RouteItem>();
                while (currentRow <= maxRow)
                {
                    var stationData = worksheet.Cells[currentRow, 2].Text?.Trim(); // Колонка B
                    
                    // Если пустая строка или новый блок (номер поезда), останавливаемся
                    if (string.IsNullOrWhiteSpace(stationData))
                    {
                        currentRow++;
                        break;
                    }

                    // Проверяем, не начался ли новый блок с поездом
                    var checkC = worksheet.Cells[currentRow, 3].Text?.Trim(); // Колонка C
                    if (string.IsNullOrWhiteSpace(checkC) && IsTrainNumber(stationData))
                    {
                        // Это новый поезд
                        break;
                    }

                    try
                    {
                        var routeItem = ParseRouteItem(worksheet, currentRow);
                        if (routeItem != null)
                        {
                            routeItems.Add(routeItem);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"⚠ Ошибка парсинга строки {currentRow}: {ex.Message}");
                    }

                    currentRow++;
                }

                if (routeItems.Any())
                {
                    var train = new Train
                    {
                        Name = trainNumber,
                        RouteItems = routeItems
                    };

                    // Определяем начальную и конечную станции
                    if (routeItems.Count > 0)
                    {
                        train.StartStation = routeItems.First().StationName ?? "";
                        train.StartStationId = routeItems.First().StationId;
                        train.EndStation = routeItems.Last().StationName ?? "";
                        train.EndStationId = routeItems.Last().StationId;
                    }

                    trains.Add(train);
                    Console.WriteLine($"✓ Загружено станций: {routeItems.Count}");
                }
            }
            else
            {
                currentRow++;
            }
        }

        Console.WriteLine($"\n✓ Всего загружено поездов: {trains.Count}");
        return trains;
    }

    private RouteItem? ParseRouteItem(ExcelWorksheet worksheet, int row)
    {
        // Структура столбцов (начиная с колонки B):
        // B: Код станции
        // C: Уровень станции
        // D: Расстояние
        // E: Время приб. на станцию
        // F: Время отпр. со станции
        // G: Дата отпр.
        // H: Город
        // I: Код дороги
        // J: Код линии
        // K: Поезд отпр. со станции
        // L: Признаки станции

        var stationCodeAndName = worksheet.Cells[row, 2].Text?.Trim(); // Колонка B
        if (string.IsNullOrWhiteSpace(stationCodeAndName))
            return null;

        // Парсим код станции и название (формат: "2700152 НУРЛЫ ЖОЛ")
        var parts = stationCodeAndName.Split(new[] { ' ' }, 2);
        string? stationCode = null;
        string? stationName = null;
        long? stationId = null;

        if (parts.Length >= 1)
        {
            stationCode = parts[0].Trim();
            if (long.TryParse(stationCode, out var code))
            {
                stationId = code;
            }
        }

        if (parts.Length >= 2)
        {
            stationName = parts[1].Trim();
        }
        else
        {
            stationName = stationCodeAndName;
        }

        // Если станция не найдена в БД, пытаемся найти по коду
        if (stationId.HasValue)
        {
            var existingStation = dbContext.Stations!
                .FirstOrDefault(s => s.Code == stationCode);
            
            if (existingStation != null)
            {
                stationId = existingStation.Id;
                stationName = existingStation.Name;
            }
        }

        var routeItem = new RouteItem
        {
            StationCode = stationCode,
            StationName = stationName,
            StationId = stationId
        };

        // Уровень станции (C)
        var levelText = worksheet.Cells[row, 3].Text?.Trim();
        // Можно сохранить при необходимости

        // Расстояние (D)
        var distanceText = worksheet.Cells[row, 4].Text?.Trim();
        if (!string.IsNullOrWhiteSpace(distanceText))
        {
            // Убираем пробелы из чисел (например, "1 005" -> "1005")
            distanceText = distanceText.Replace(" ", "").Replace("\u00A0", "");
            if (double.TryParse(distanceText, NumberStyles.Any, CultureInfo.InvariantCulture, out var distance))
            {
                routeItem.Distance = distance.ToString(CultureInfo.InvariantCulture);
                routeItem.DistanceKm = distance;
            }
        }

        // Время прибытия (E)
        var arrivalText = worksheet.Cells[row, 5].Text?.Trim();
        if (!string.IsNullOrWhiteSpace(arrivalText) && arrivalText != "00:00:00")
        {
            routeItem.Arrival = arrivalText;
            if (TimeSpan.TryParse(arrivalText, out var arrivalTime))
            {
                routeItem.ArrivalTime = arrivalTime;
            }
        }

        // Время отправления (F)
        var departureText = worksheet.Cells[row, 6].Text?.Trim();
        if (!string.IsNullOrWhiteSpace(departureText) && departureText != "00:00:00")
        {
            routeItem.Departure = departureText;
            if (TimeSpan.TryParse(departureText, out var departureTime))
            {
                routeItem.DepartureTime = departureTime;
            }
        }

        // Вычисляем время стоянки
        if (routeItem.ArrivalTime.HasValue && routeItem.DepartureTime.HasValue)
        {
            var stopTime = routeItem.DepartureTime.Value - routeItem.ArrivalTime.Value;
            if (stopTime.TotalMinutes > 0)
            {
                routeItem.StopMinutes = (int)stopTime.TotalMinutes;
                routeItem.Stop = $"{routeItem.StopMinutes} мин";
            }
        }

        // Дата отправления (G)
        // Код дороги (I)
        // Код линии (J)
        // Поезд отпр. со станции (K)
        // Признаки станции (L)
        // Эти поля можно добавить в RouteItem при необходимости

        return routeItem;
    }

    /// <summary>
    /// Проверяет, является ли строка номером поезда (только цифры)
    /// </summary>
    private bool IsTrainNumber(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return false;

        return text.All(c => char.IsDigit(c));
    }
}

