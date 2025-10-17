using Data.Loading.Models;
using Microsoft.EntityFrameworkCore;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Data.TicketDb.Entities.Dictionaries;

namespace Data.Loading.Services;

/// <summary>
/// Сервис для сохранения данных о поездах в БД
/// </summary>
public class TrainsDatabaseService
{
    private readonly TicketDbContext dbContext;
    
    // Маппинг для особых случаев (расписание => БД)
    private readonly Dictionary<string, string> trainNumberMapping = new()
    {
        { "0077Т", "78" }
    };

    public TrainsDatabaseService(TicketDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <summary>
    /// Обновляет расстояния для поездов из расписания
    /// </summary>
    public async Task UpdateDistancesAsync(List<Models.Train> scheduleTrains)
    {
        Console.WriteLine("\n=== ОБНОВЛЕНИЕ РАССТОЯНИЙ ===");
        
        var updatedTrains = 0;
        var updatedDistances = 0;
        var errors = new List<string>();

        foreach (var scheduleTrain in scheduleTrains)
        {
            try
            {
                // Нормализуем номер поезда (убираем ведущие нули и буквы)
                var normalizedScheduleNumber = NormalizeTrainNumber(scheduleTrain.Name);
                
                // Загружаем все поезда и ищем по нормализованному номеру
                var allTrains = await dbContext.Trains!.ToListAsync();
                var train = allTrains.FirstOrDefault(t => NormalizeTrainNumber(t.Name) == normalizedScheduleNumber);

                if (train == null)
                {
                    var error = $"❌ Поезд '{scheduleTrain.Name}' (нормализовано: '{normalizedScheduleNumber}') не найден в БД";
                    errors.Add(error);
                    Console.WriteLine(error);
                    continue;
                }

                // Находим маршрут для этого поезда
                var route = await dbContext.Routes!
                    .FirstOrDefaultAsync(r => r.TrainId == train.Id);

                if (route == null)
                {
                    var error = $"❌ Маршрут для поезда '{scheduleTrain.Name}' не найден в БД";
                    errors.Add(error);
                    Console.WriteLine(error);
                    continue;
                }

                // Загружаем все станции маршрута
                var routeStations = await dbContext.RouteStations!
                    .Where(rs => rs.RouteId == route.Id)
                    .Include(rs => rs.Station)
                    .ToListAsync();

                // Сбрасываем все расстояния в 0
                foreach (var rs in routeStations)
                {
                    rs.Distance = 0;
                }

                // Обновляем расстояния из расписания
                foreach (var routeItem in scheduleTrain.RouteItems)
                {
                    if (!routeItem.StationId.HasValue)
                        continue;

                    var routeStation = routeStations.FirstOrDefault(rs => rs.StationId == routeItem.StationId.Value);

                    if (routeStation == null)
                    {
                        var stationCode = routeItem.StationCode ?? "N/A";
                        var stationName = routeItem.StationName ?? "N/A";
                        var error = $"❌ Станция (Код: {stationCode}, Имя: {stationName}, ID: {routeItem.StationId}) не найдена в маршруте поезда '{scheduleTrain.Name}'";
                        errors.Add(error);
                        Console.WriteLine(error);
                        continue;
                    }

                    routeStation.Distance = routeItem.DistanceKm ?? 0;
                    updatedDistances++;
                }

                updatedTrains++;
                Console.WriteLine($"✓ Обновлено расстояний для поезда '{scheduleTrain.Name}' (БД: '{train.Name}'): {routeStations.Count} станций");
            }
            catch (Exception ex)
            {
                var error = $"❌ Ошибка при обработке поезда '{scheduleTrain.Name}': {ex.Message}";
                errors.Add(error);
                Console.WriteLine(error);
            }
        }

        await dbContext.SaveChangesAsync();

        Console.WriteLine($"\n✓ Обновлено поездов: {updatedTrains}");
        Console.WriteLine($"✓ Обновлено расстояний: {updatedDistances}");
        
        if (errors.Any())
        {
            Console.WriteLine($"\n⚠ Всего ошибок: {errors.Count}");
        }
    }

    /// <summary>
    /// Сохраняет все данные о поездах в БД
    /// </summary>
    public async Task SaveTrainsAsync(TrainsLoadResult result)
    {
        Console.WriteLine("\n=== СОХРАНЕНИЕ В БД ===");
        
        var savedTrains = 0;
        var savedRoutes = 0;
        var savedRouteStations = 0;
        var savedWagonModels = 0;
        var savedSeats = 0;
        var savedSeatTypes = 0;
        var savedTrainTypes = 0;
        var savedWagonPlans = 0;
        var savedPlanWagons = 0;

        foreach (var routeData in result.Routes)
        {
            // Сохраняем Train1
            if (!string.IsNullOrEmpty(routeData.Train1.Name))
            {
                var stats = await SaveTrainWithRouteAsync(routeData.Train1, routeData.Name);
                savedTrains += stats.Trains;
                savedRoutes += stats.Routes;
                savedRouteStations += stats.RouteStations;
                savedWagonModels += stats.WagonModels;
                savedSeats += stats.Seats;
                savedSeatTypes += stats.SeatTypes;
                savedTrainTypes += stats.TrainTypes;
                savedWagonPlans += stats.WagonPlans;
                savedPlanWagons += stats.PlanWagons;
            }

            // Сохраняем Train2
            if (!string.IsNullOrEmpty(routeData.Train2.Name))
            {
                var stats = await SaveTrainWithRouteAsync(routeData.Train2, routeData.Name);
                savedTrains += stats.Trains;
                savedRoutes += stats.Routes;
                savedRouteStations += stats.RouteStations;
                savedWagonModels += stats.WagonModels;
                savedSeats += stats.Seats;
                savedSeatTypes += stats.SeatTypes;
                savedTrainTypes += stats.TrainTypes;
                savedWagonPlans += stats.WagonPlans;
                savedPlanWagons += stats.PlanWagons;
            }
        }

        await dbContext.SaveChangesAsync();

        Console.WriteLine($"✓ Сохранено поездов: {savedTrains}");
        Console.WriteLine($"✓ Сохранено маршрутов: {savedRoutes}");
        Console.WriteLine($"✓ Сохранено станций маршрутов: {savedRouteStations}");
        Console.WriteLine($"✓ Сохранено типов поездов: {savedTrainTypes}");
        Console.WriteLine($"✓ Сохранено планов вагонов: {savedWagonPlans}");
        Console.WriteLine($"✓ Сохранено вагонов в планах: {savedPlanWagons}");
        Console.WriteLine($"✓ Сохранено моделей вагонов: {savedWagonModels}");
        Console.WriteLine($"✓ Сохранено типов мест: {savedSeatTypes}");
        Console.WriteLine($"✓ Сохранено мест: {savedSeats}");
    }

    private async Task<SaveStats> SaveTrainWithRouteAsync(Models.Train trainData, string routeName)
    {
        var stats = new SaveStats();

        // Определяем тип поезда (на основе имени/номера)
        var trainType = await GetOrCreateTrainTypeAsync(trainData.Name);
        if (trainType != null)
        {
            stats.TrainTypes++;
        }

        // Проверяем существует ли поезд с таким именем
        var train = await dbContext.Trains!
            .FirstOrDefaultAsync(t => t.Name == trainData.Name);

        if (train == null)
        {
            train = new Ticketing.Data.TicketDb.Entities.Train
            {
                Name = trainData.Name,
                FromId = trainData.StartStationId,
                ToId = trainData.EndStationId,
                TypeId = trainType?.Id,
                ZoneType = 0, // TODO: определить из данных
                Importance = 1,
                Amenities = 0
            };
            dbContext.Trains!.Add(train);
            stats.Trains++;
        }
        else
        {
            // Обновляем существующий поезд
            train.FromId = trainData.StartStationId;
            train.ToId = trainData.EndStationId;
            train.TypeId = trainType?.Id;
            dbContext.Trains!.Update(train);
        }
        await dbContext.SaveChangesAsync();

        // Проверяем существует ли маршрут
        var route = await dbContext.Routes!
            .FirstOrDefaultAsync(r => r.Name == routeName && r.TrainId == train.Id);

        if (route == null)
        {
            route = new Ticketing.Data.TicketDb.Entities.Route
            {
                Name = routeName,
                TrainId = train.Id
            };
            dbContext.Routes!.Add(route);
            stats.Routes++;
        }
        else
        {
            // Обновляем существующий маршрут
            route.Name = routeName;
            route.TrainId = train.Id;
            dbContext.Routes!.Update(route);
        }
        await dbContext.SaveChangesAsync();

        // Создаем/обновляем станции маршрута
        var order = 1;
        var baseDate = DateTime.UtcNow.Date; // Используем UTC для PostgreSQL
        foreach (var routeItem in trainData.RouteItems)
        {
            if (routeItem.StationId.HasValue)
            {
                var existingRouteStation = await dbContext.RouteStations!
                    .FirstOrDefaultAsync(rs => rs.RouteId == route.Id && rs.StationId == routeItem.StationId.Value);

                var arrival = routeItem.ArrivalTime.HasValue 
                    ? DateTime.SpecifyKind(baseDate.Add(routeItem.ArrivalTime.Value), DateTimeKind.Utc)
                    : (DateTime?)null;
                var departure = routeItem.DepartureTime.HasValue 
                    ? DateTime.SpecifyKind(baseDate.Add(routeItem.DepartureTime.Value), DateTimeKind.Utc)
                    : (DateTime?)null;
                var stop = routeItem.StopMinutes.HasValue && arrival.HasValue
                    ? arrival.Value.AddMinutes(routeItem.StopMinutes.Value)
                    : (DateTime?)null;

                if (existingRouteStation == null)
                {
                    var routeStation = new RouteStation
                    {
                        RouteId = route.Id,
                        StationId = routeItem.StationId.Value,
                        Order = order,
                        Arrival = arrival,
                        Departure = departure,
                        Stop = stop,
                        Distance = routeItem.DistanceKm ?? 0
                    };
                    dbContext.RouteStations!.Add(routeStation);
                    stats.RouteStations++;
                }
                else
                {
                    // Обновляем существующую станцию
                    existingRouteStation.Order = order;
                    existingRouteStation.Arrival = arrival;
                    existingRouteStation.Departure = departure;
                    existingRouteStation.Stop = stop;
                    existingRouteStation.Distance = routeItem.DistanceKm ?? 0;
                    dbContext.RouteStations!.Update(existingRouteStation);
                }

                order++;
            }
        }
        await dbContext.SaveChangesAsync();

        // Сохраняем вагоны и места
        var wagonStats = await SaveWagonsAndSeatsAsync(trainData.Wagons);
        stats.WagonModels += wagonStats.WagonModels;
        stats.Seats += wagonStats.Seats;
        stats.SeatTypes += wagonStats.SeatTypes;

        // Создаем план вагонов для поезда
        if (trainData.Wagons.Any())
        {
            var planStats = await CreateTrainWagonsPlanAsync(train.Id, trainData.Name, trainData.Wagons);
            stats.WagonPlans += planStats.WagonPlans;
            stats.PlanWagons += planStats.PlanWagons;
        }

        return stats;
    }

    private async Task<SaveStats> SaveWagonsAndSeatsAsync(List<Models.TrainWagon> wagons)
    {
        var stats = new SaveStats();

        foreach (var wagon in wagons)
        {
            // Проверяем существует ли модель вагона с таким типом
            var wagonModel = await dbContext.WagonModels!
                .FirstOrDefaultAsync(w => w.Name == wagon.Type);

            // Подсчитываем общее количество мест
            var totalSeats = wagon.SeatCounts
                .Sum(sc => int.TryParse(sc.Count, out var count) ? count : 0);

            if (wagonModel == null)
            {
                wagonModel = new WagonModel
                {
                    Name = wagon.Type,
                    SeatCount = totalSeats,
                    HasLiftingMechanism = false
                };
                dbContext.WagonModels!.Add(wagonModel);
                await dbContext.SaveChangesAsync();
                stats.WagonModels++;
            }
            else
            {
                // Обновляем количество мест, если изменилось
                if (wagonModel.SeatCount != totalSeats)
                {
                    wagonModel.SeatCount = totalSeats;
                    dbContext.WagonModels!.Update(wagonModel);
                    await dbContext.SaveChangesAsync();
                }
            }

            // Создаем/обновляем типы мест и места
            foreach (var seatCount in wagon.SeatCounts)
            {
                if (int.TryParse(seatCount.Count, out var count) && count > 0)
                {
                    // Проверяем/создаем тип места
                    var (seatType, isNew) = await GetOrCreateSeatTypeAsync(seatCount.Name);
                    if (seatType != null)
                    {
                        if (isNew) stats.SeatTypes++;

                        // Проверяем сколько мест уже есть для этого типа
                        var existingSeatsCount = await dbContext.Seats!
                            .CountAsync(s => s.WagonId == wagonModel.Id && s.TypeId == seatType.Id);

                        // Создаем недостающие места
                        for (int i = existingSeatsCount + 1; i <= count; i++)
                        {
                            var existingSeat = await dbContext.Seats!
                                .FirstOrDefaultAsync(s => s.WagonId == wagonModel.Id && s.Number == i.ToString() && s.TypeId == seatType.Id);

                            if (existingSeat == null)
                            {
                                var seat = new Seat
                                {
                                    WagonId = wagonModel.Id,
                                    TypeId = seatType.Id,
                                    Number = i.ToString(),
                                    Class = 2 // TODO: определить из данных
                                };
                                dbContext.Seats!.Add(seat);
                                stats.Seats++;
                            }
                        }
                    }
                }
            }
        }

        await dbContext.SaveChangesAsync();
        return stats;
    }

    private async Task<(SeatType? seatType, bool isNew)> GetOrCreateSeatTypeAsync(string name)
    {
        if (string.IsNullOrEmpty(name))
            return (null, false);

        var seatType = await dbContext.SeatTypes!
            .FirstOrDefaultAsync(st => st.Name == name);

        if (seatType == null)
        {
            seatType = new SeatType
            {
                Name = name,
                Code = GenerateSeatTypeCode(name),
                TarifCoefficient = 1.0
            };
            dbContext.SeatTypes!.Add(seatType);
            await dbContext.SaveChangesAsync();
            return (seatType, true);
        }

        return (seatType, false);
    }

    private string GenerateSeatTypeCode(string name)
    {
        // Простая генерация кода на основе первых букв
        var words = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (words.Length == 0)
            return "UNK";

        return string.Join("", words.Take(3).Select(w => w[0])).ToUpper();
    }

    private async Task<TrainType?> GetOrCreateTrainTypeAsync(string trainName)
    {
        if (string.IsNullOrEmpty(trainName))
            return null;

        // Определяем тип поезда на основе номера/имени
        // Скорые поезда обычно имеют номера 1-150
        // Пассажирские 151-300
        // Пригородные 301+
        var trainTypeName = DetermineTrainTypeName(trainName);
        var trainTypeCode = DetermineTrainTypeCode(trainName);

        var trainType = await dbContext.Set<TrainType>()
            .FirstOrDefaultAsync(tt => tt.Code == trainTypeCode);

        if (trainType == null)
        {
            trainType = new TrainType
            {
                Name = trainTypeName,
                Code = trainTypeCode
            };
            dbContext.Set<TrainType>().Add(trainType);
            await dbContext.SaveChangesAsync();
        }

        return trainType;
    }

    private string DetermineTrainTypeName(string trainName)
    {
        // Извлекаем номер из имени поезда
        if (int.TryParse(new string(trainName.TakeWhile(char.IsDigit).ToArray()), out var trainNumber))
        {
            if (trainNumber <= 150)
                return "Скорый";
            else if (trainNumber <= 300)
                return "Пассажирский";
            else
                return "Пригородный";
        }

        return "Неопределенный";
    }

    private string DetermineTrainTypeCode(string trainName)
    {
        var typeName = DetermineTrainTypeName(trainName);
        return typeName switch
        {
            "Скорый" => "EXPR",
            "Пассажирский" => "PASS",
            "Пригородный" => "SUB",
            _ => "UNK"
        };
    }

    private async Task<SaveStats> CreateTrainWagonsPlanAsync(long trainId, string trainName, List<Models.TrainWagon> wagons)
    {
        var stats = new SaveStats();

        // Проверяем существует ли план для этого поезда
        var plan = await dbContext.TrainWagonsPlans!
            .Include(p => p.Wagons)
            .FirstOrDefaultAsync(p => p.TrainId == trainId);

        var planName = $"План состава для поезда {trainName}";

        if (plan == null)
        {
            plan = new TrainWagonsPlan
            {
                Name = planName,
                TrainId = trainId
            };
            dbContext.TrainWagonsPlans!.Add(plan);
            await dbContext.SaveChangesAsync();
            stats.WagonPlans++;
        }
        else
        {
            // Обновляем название плана
            plan.Name = planName;
            dbContext.TrainWagonsPlans!.Update(plan);
            await dbContext.SaveChangesAsync();
        }

        // Добавляем/обновляем вагоны в план
        foreach (var wagon in wagons)
        {
            // Ищем модель вагона
            var wagonModel = await dbContext.WagonModels!
                .FirstOrDefaultAsync(w => w.Name == wagon.Type);

            if (wagonModel != null)
            {
                // Проверяем существует ли вагон в плане
                var existingPlanWagon = await dbContext.TrainWagonsPlanWagons!
                    .FirstOrDefaultAsync(pw => pw.PlanId == plan.Id && pw.Number == wagon.Number);

                if (existingPlanWagon == null)
                {
                    var planWagon = new TrainWagonsPlanWagon
                    {
                        PlanId = plan.Id,
                        WagonId = wagonModel.Id,
                        Number = wagon.Number
                    };
                    dbContext.TrainWagonsPlanWagons!.Add(planWagon);
                    stats.PlanWagons++;
                }
                else
                {
                    // Обновляем существующий вагон
                    existingPlanWagon.WagonId = wagonModel.Id;
                    existingPlanWagon.Number = wagon.Number;
                    dbContext.TrainWagonsPlanWagons!.Update(existingPlanWagon);
                }
            }
        }

        await dbContext.SaveChangesAsync();
        return stats;
    }

    /// <summary>
    /// Нормализует номер поезда: проверяет кастомный маппинг, затем убирает ведущие нули
    /// Например: "0077Т" -> "78" (по маппингу), "026T" -> "26", "001А" -> "1", "123" -> "123"
    /// </summary>
    private string NormalizeTrainNumber(string trainName)
    {
        if (string.IsNullOrEmpty(trainName))
            return string.Empty;

        // Проверяем кастомный маппинг
        if (trainNumberMapping.TryGetValue(trainName, out var mappedNumber))
            return mappedNumber;

        // Извлекаем только цифры из начала строки
        var digits = new string(trainName.TakeWhile(char.IsDigit).ToArray());
        
        if (string.IsNullOrEmpty(digits))
            return trainName; // Если нет цифр, возвращаем как есть
        
        // Убираем ведущие нули
        var number = int.Parse(digits);
        return number.ToString();
    }

    private class SaveStats
    {
        public int Trains { get; set; }
        public int Routes { get; set; }
        public int RouteStations { get; set; }
        public int WagonModels { get; set; }
        public int Seats { get; set; }
        public int SeatTypes { get; set; }
        public int TrainTypes { get; set; }
        public int WagonPlans { get; set; }
        public int PlanWagons { get; set; }
    }
}

