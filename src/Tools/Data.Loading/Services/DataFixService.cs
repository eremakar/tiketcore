using Microsoft.EntityFrameworkCore;
using Ticketing.Data.TicketDb.DatabaseContext;

namespace Data.Loading.Services;

/// <summary>
/// Сервис для исправления данных в БД
/// </summary>
public class DataFixService
{
    private readonly TicketDbContext dbContext;

    public DataFixService(TicketDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <summary>
    /// Удаляет вагоны с некорректными номерами (заканчиваются на 'ф' или 'Всего:' или 'В С Е Г О:')
    /// </summary>
    public async Task DeleteInvalidWagonsAsync()
    {
        Console.WriteLine("\n=== УДАЛЕНИЕ НЕКОРРЕКТНЫХ ВАГОНОВ ===");

        var planWagons = await dbContext.TrainWagonsPlanWagons!.ToListAsync();

        var toDelete = planWagons.Where(pw =>
            pw.Number != null &&
            (pw.Number.EndsWith("ф", StringComparison.OrdinalIgnoreCase) ||
             pw.Number == "Всего:" ||
             pw.Number == "В С Е Г О:" ||
             pw.Number == "Всего по дороге:" ||
             pw.Number == "И Т О Г О:" ||
             pw.Number == "ВСЕГО:"))
            .ToList();

        Console.WriteLine($"Найдено некорректных вагонов: {toDelete.Count}");

        foreach (var wagon in toDelete)
        {
            Console.WriteLine($"  Удаление: PlanId={wagon.PlanId}, Number='{wagon.Number}'");
        }

        dbContext.TrainWagonsPlanWagons!.RemoveRange(toDelete);
        await dbContext.SaveChangesAsync();

        Console.WriteLine($"✓ Удалено вагонов: {toDelete.Count}");
    }

    /// <summary>
    /// Удаляет RouteStation где Stop = null
    /// </summary>
    public async Task DeleteRouteStationsWithNullStopAsync()
    {
        Console.WriteLine("\n=== УДАЛЕНИЕ ROUTESTATION С NULL STOP ===");

        var routeStations = await dbContext.RouteStations!
            .Where(rs => rs.Stop == null && rs.RouteId >= 4)
            .ToListAsync();

        Console.WriteLine($"Найдено RouteStation с null Stop: {routeStations.Count}");

        dbContext.RouteStations!.RemoveRange(routeStations);
        await dbContext.SaveChangesAsync();

        Console.WriteLine($"✓ Удалено RouteStation: {routeStations.Count}");
    }

    /// <summary>
    /// Корректирует Stop (минус 5 часов)
    /// </summary>
    public async Task FixStopTimesAsync()
    {
        Console.WriteLine("\n=== КОРРЕКТИРОВКА STOP (МИНУС 5 ЧАСОВ) ===");

        var routeStations = await dbContext.RouteStations!.ToListAsync();

        var updated = 0;

        foreach (var rs in routeStations)
        {
            if (rs.RouteId < 4)
                continue;

            if (rs.Stop.HasValue)
            {
                rs.Stop = rs.Stop.Value.AddDays(1).AddHours(-5);
                updated++;
            }
        }

        await dbContext.SaveChangesAsync();

        Console.WriteLine($"✓ Обновлено станций: {updated}");
    }

    /// <summary>
    /// Корректирует время прибытия/отправления (минус 5 часов) и пересчитывает стоянку
    /// </summary>
    public async Task FixRouteStationTimesAsync()
    {
        Console.WriteLine("\n=== КОРРЕКТИРОВКА ВРЕМЕНИ СТАНЦИЙ ===");

        var routes = await dbContext.Routes!
            .Include(r => r.Stations)
            .ToListAsync();

        var updatedRoutes = 0;
        var updatedStations = 0;

        foreach (var route in routes)
        {
            if (route.Id < 4)
                continue;

            if (route.Stations == null || !route.Stations.Any())
                continue;

            var routeHasChanges = false;

            foreach (var rs in route.Stations)
            {
                var hasChanges = false;

                // Корректируем arrival (-5 часов)
                if (rs.Arrival.HasValue)
                {
                    rs.Arrival = rs.Arrival.Value.AddHours(-5);
                    hasChanges = true;
                }

                // Корректируем departure (-5 часов)
                if (rs.Departure.HasValue)
                {
                    rs.Departure = rs.Departure.Value.AddHours(-5);
                    hasChanges = true;
                }

            // Пересчитываем Stop = продолжительность остановки в минутах (как DateTime от минимальной даты)
            if (rs.Arrival.HasValue && rs.Departure.HasValue)
            {
                var stopDurationMinutes = (rs.Departure.Value - rs.Arrival.Value).TotalMinutes;
                    try
                    {
                        rs.Stop = DateTime.SpecifyKind(DateTime.MinValue.AddMinutes(stopDurationMinutes), DateTimeKind.Utc);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        Console.WriteLine($"⚠ Ошибка при вычислении Stop для RouteStation ID={rs.Id} (Arrival: {rs.Arrival}, Departure: {rs.Departure})");
                        rs.Stop = null;
                    }
                    hasChanges = true;
            }
            else
            {
                rs.Stop = null;
            }

                if (hasChanges)
                {
                    updatedStations++;
                    routeHasChanges = true;
                }
            }

            if (routeHasChanges)
            {
                updatedRoutes++;
            }
        }

        await dbContext.SaveChangesAsync();

        Console.WriteLine($"✓ Обновлено маршрутов: {updatedRoutes}");
        Console.WriteLine($"✓ Обновлено станций: {updatedStations}");
    }

    /// <summary>
    /// Исправляет имена маршрутов на формат: "номер поезда <Станция от>-<Станция до>"
    /// </summary>
    public async Task FixRouteNamesAsync()
    {
        Console.WriteLine("\n=== ИСПРАВЛЕНИЕ ИМЕН МАРШРУТОВ ===");

        var routes = await dbContext.Routes!
            .Include(r => r.Train)
            .ThenInclude(t => t!.From)
            .Include(r => r.Train)
            .ThenInclude(t => t!.To)
            .ToListAsync();

        var updated = 0;
        var errors = 0;

        foreach (var route in routes)
        {
            try
            {
                if (route.Train == null)
                {
                    Console.WriteLine($"⚠ Маршрут ID={route.Id} не имеет поезда");
                    errors++;
                    continue;
                }

                var trainNumber = route.Train.Name ?? "N/A";
                var fromStation = route.Train.From?.Name ?? "Неизвестно";
                var toStation = route.Train.To?.Name ?? "Неизвестно";

                var newName = $"{trainNumber} {fromStation}-{toStation}";

                if (route.Name != newName)
                {
                    Console.WriteLine($"Обновление: '{route.Name}' => '{newName}'");
                    route.Name = newName;
                    updated++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка при обработке маршрута ID={route.Id}: {ex.Message}");
                errors++;
            }
        }

        await dbContext.SaveChangesAsync();

        Console.WriteLine($"\n✓ Обновлено маршрутов: {updated}");
        if (errors > 0)
        {
            Console.WriteLine($"⚠ Ошибок: {errors}");
        }
    }

    /// <summary>
    /// Восстанавливает первые и конечные станции в маршрутах из загруженных данных
    /// </summary>
    public async Task RestoreFirstAndLastStationsAsync(Data.Loading.Models.TrainsLoadResult loadedData)
    {
        Console.WriteLine("\n=== ВОССТАНОВЛЕНИЕ ПЕРВЫХ И КОНЕЧНЫХ СТАНЦИЙ ===");

        var routes = await dbContext.Routes!
            .Include(r => r.Train)
            .Include(r => r.Stations)
            .Where(r => r.Id >= 4)
            .ToListAsync();

        var addedFirst = 0;
        var addedLast = 0;
        var errors = 0;

        foreach (var route in routes)
        {
            try
            {
                if (route.Train == null)
                {
                    Console.WriteLine($"⚠ Маршрут ID={route.Id} не имеет поезда");
                    errors++;
                    continue;
                }

                // Находим соответствующий маршрут в загруженных данных по имени поезда
                var loadedRoute = loadedData.Routes
                    .SelectMany(r => new[] { r.Train1, r.Train2 })
                    .FirstOrDefault(t => t.Name == route.Train.Name);

                if (loadedRoute == null)
                {
                    Console.WriteLine($"⚠ Маршрут '{route.Train.Name}' не найден в загруженных данных");
                    errors++;
                    continue;
                }

                //if (loadedRoute.StartStationId == null || loadedRoute.EndStationId == null)
                //{
                //    Console.WriteLine($"⚠ Маршрут '{route.Train.Name}': не указаны StationId для первой/конечной станции");
                //    errors++;
                //    continue;
                //}

                if (loadedRoute.RouteItems == null || !loadedRoute.RouteItems.Any())
                {
                    Console.WriteLine($"⚠ Маршрут '{route.Train.Name}': нет RouteItems");
                    errors++;
                    continue;
                }

                route.Stations ??= new List<Ticketing.Data.TicketDb.Entities.RouteStation>();

                var firstRouteItem = loadedRoute.RouteItems.First();
                var lastRouteItem = loadedRoute.RouteItems.Last();

                // Проверяем первую станцию
                var hasFirstStation = route.Stations.Any(rs => rs.StationId == loadedRoute.StartStationId);
                if (!hasFirstStation)
                {
                    var minOrder = route.Stations.Any() ? route.Stations.Min(rs => rs.Order) : 1;
                    
                    var firstStation = new Ticketing.Data.TicketDb.Entities.RouteStation
                    {
                        RouteId = route.Id,
                        StationId = loadedRoute.StartStationId,
                        Order = minOrder > 1 ? 1 : minOrder - 1,
                        Distance = 0,
                        Arrival = null,
                        Departure = firstRouteItem.DepartureTime.HasValue 
                            ? DateTime.SpecifyKind(DateTime.MinValue.Add(firstRouteItem.DepartureTime.Value), DateTimeKind.Utc)
                            : null,
                        Stop = null
                    };
                    
                    dbContext.RouteStations!.Add(firstStation);
                    addedFirst++;
                    Console.WriteLine($"  ✓ Добавлена первая станция для маршрута '{route.Train.Name}' (StationId={loadedRoute.StartStationId}, Station={firstRouteItem.StationName}, Order={firstStation.Order})");
                }

                // Проверяем последнюю станцию
                var hasLastStation = route.Stations.Any(rs => rs.StationId == loadedRoute.EndStationId);
                if (!hasLastStation)
                {
                    var maxOrder = route.Stations.Any() ? route.Stations.Max(rs => rs.Order) : 0;
                    var maxDistance = lastRouteItem.DistanceKm ?? 0;
                    
                    var lastStation = new Ticketing.Data.TicketDb.Entities.RouteStation
                    {
                        RouteId = route.Id,
                        StationId = loadedRoute.EndStationId,
                        Order = maxOrder + 1,
                        Distance = maxDistance,
                        Arrival = lastRouteItem.ArrivalTime.HasValue 
                            ? DateTime.SpecifyKind(DateTime.MinValue.Add(lastRouteItem.ArrivalTime.Value), DateTimeKind.Utc)
                            : null,
                        Departure = null,
                        Stop = null
                    };
                    
                    dbContext.RouteStations!.Add(lastStation);
                    addedLast++;
                    Console.WriteLine($"  ✓ Добавлена конечная станция для маршрута '{route.Train.Name}' (StationId={loadedRoute.EndStationId}, Station={lastRouteItem.StationName}, Order={lastStation.Order})");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка при обработке маршрута ID={route.Id}: {ex.Message}");
                errors++;
            }
        }

        await dbContext.SaveChangesAsync();

        Console.WriteLine($"\n✓ Добавлено первых станций: {addedFirst}");
        Console.WriteLine($"✓ Добавлено конечных станций: {addedLast}");
        if (errors > 0)
        {
            Console.WriteLine($"⚠ Ошибок: {errors}");
        }
    }

    /// <summary>
    /// Восстанавливает время прибытия/отправления для всех станций из загруженных данных
    /// </summary>
    public async Task RestoreAllStationsTimesAsync(Data.Loading.Models.TrainsLoadResult loadedData)
    {
        Console.WriteLine("\n=== ВОССТАНОВЛЕНИЕ ВРЕМЕНИ ДЛЯ ВСЕХ СТАНЦИЙ ===");

        var routes = await dbContext.Routes!
            .Include(r => r.Train)
            .Include(r => r.Stations)
            .Where(r => r.Id >= 4)
            .ToListAsync();

        var updatedRoutes = 0;
        var updatedStations = 0;
        var notFoundStations = 0;
        var errors = 0;
        var baseDate = DateTime.UtcNow.Date; // Используем UTC для PostgreSQL

        foreach (var route in routes)
        {
            try
            {
                if (route.Train == null)
                {
                    Console.WriteLine($"⚠ Маршрут ID={route.Id} не имеет поезда");
                    errors++;
                    continue;
                }

                // Находим соответствующий маршрут в загруженных данных по имени поезда
                var loadedRoute = loadedData.Routes
                    .SelectMany(r => new[] { r.Train1, r.Train2 })
                    .FirstOrDefault(t => t.Name == route.Train.Name);

                if (loadedRoute == null)
                {
                    Console.WriteLine($"⚠ Маршрут '{route.Train.Name}' не найден в загруженных данных");
                    errors++;
                    continue;
                }

                if (loadedRoute.RouteItems == null || !loadedRoute.RouteItems.Any())
                {
                    Console.WriteLine($"⚠ Маршрут '{route.Train.Name}': нет RouteItems");
                    errors++;
                    continue;
                }

                if (route.Stations == null || !route.Stations.Any())
                {
                    Console.WriteLine($"⚠ Маршрут '{route.Train.Name}': нет станций в БД");
                    errors++;
                    continue;
                }

                var routeHasChanges = false;

                foreach (var routeStation in route.Stations)
                {
                    var routeItem = loadedRoute.RouteItems.FirstOrDefault(ri => ri.StationId == routeStation.StationId);
                    
                    if (routeItem == null)
                    {
                        notFoundStations++;
                        continue;
                    }

                    var hasChanges = false;

                    routeStation.Day = routeItem.Day ?? 0;
                    if (routeItem.Day > 0)
                    {

                    }

                    // Обновляем Arrival (минус 5 часов)
                    // var newArrival = routeItem.ArrivalTime.HasValue
                    //     ? DateTime.SpecifyKind(baseDate.Add(routeItem.ArrivalTime.Value).AddHours(-5), DateTimeKind.Utc)
                    //     : (DateTime?)null;

                    // if (routeStation.Arrival != newArrival)
                    // {
                    //     routeStation.Arrival = newArrival;
                    //     hasChanges = true;
                    // }

                    // // Обновляем Departure (минус 5 часов)
                    // var newDeparture = routeItem.DepartureTime.HasValue
                    //     ? DateTime.SpecifyKind(baseDate.Add(routeItem.DepartureTime.Value).AddHours(-5), DateTimeKind.Utc)
                    //     : (DateTime?)null;

                    // if (routeStation.Departure != newDeparture)
                    // {
                    //     routeStation.Departure = newDeparture;
                    //     hasChanges = true;
                    // }

                    // Пересчитываем Stop
                    //if (routeStation.Arrival.HasValue && routeStation.Departure.HasValue)
                    //{
                    //    var stopDurationMinutes = (routeStation.Departure.Value - routeStation.Arrival.Value).TotalMinutes;
                    //    try
                    //    {
                    //        var newStop = DateTime.SpecifyKind(DateTime.MinValue.AddMinutes(stopDurationMinutes), DateTimeKind.Utc);
                    //        if (routeStation.Stop != newStop)
                    //        {
                    //            routeStation.Stop = newStop;
                    //            hasChanges = true;
                    //        }
                    //    }
                    //    catch (ArgumentOutOfRangeException)
                    //    {
                    //        Console.WriteLine($"⚠ Ошибка при вычислении Stop для RouteStation ID={routeStation.Id} (Arrival: {routeStation.Arrival}, Departure: {routeStation.Departure})");
                    //        routeStation.Stop = null;
                    //    }
                    //}
                    //else
                    //{
                    //    if (routeStation.Stop != null)
                    //    {
                    //        routeStation.Stop = null;
                    //        hasChanges = true;
                    //    }
                    //}

                    if (hasChanges)
                    {
                        updatedStations++;
                        routeHasChanges = true;
                    }
                }

                if (routeHasChanges)
                {
                    updatedRoutes++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка при обработке маршрута ID={route.Id}: {ex.Message}");
                errors++;
            }
        }

        await dbContext.SaveChangesAsync();

        Console.WriteLine($"\n✓ Обновлено маршрутов: {updatedRoutes}");
        Console.WriteLine($"✓ Обновлено станций: {updatedStations}");
        if (notFoundStations > 0)
        {
            Console.WriteLine($"⚠ Станций не найдено в загруженных данных: {notFoundStations}");
        }
        if (errors > 0)
        {
            Console.WriteLine($"⚠ Ошибок: {errors}");
        }
    }
}

