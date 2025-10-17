using Data.Loading.Models;
using Microsoft.EntityFrameworkCore;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities;

namespace Data.Loading;

public class RouteItemParser
{
    private readonly List<Station> stations;
    private readonly TicketDbContext dbContext;

    public RouteItemParser(TicketDbContext dbContext)
    {
        this.dbContext = dbContext;
        stations = dbContext.Stations!.ToList();
    }

    public void ParseRoutes(List<RouteData> routes)
    {
        foreach (var route in routes)
        {
            ParseTrain(route.Train1);
            ParseTrain(route.Train2);
        }
    }

    private void ParseTrain(Models.Train train)
    {
        if (train.Name.Contains("044"))
        {

        }

        // Определяем StartStationId и EndStationId по именам станций
        if (!string.IsNullOrWhiteSpace(train.StartStation))
        {
            train.StartStationId = GetStationIdByName(train.StartStation);
        }

        if (!string.IsNullOrWhiteSpace(train.EndStation))
        {
            train.EndStationId = GetStationIdByName(train.EndStation);
        }

        ParseRouteItems(train.RouteItems);
    }

    private long? GetStationIdByName(string stationName)
    {
        stationName = stationName.Replace(" I", "-1");

        var station = stations.FirstOrDefault(s => s.Name?.ToUpperInvariant() == 
            stationName?.ToUpperInvariant());

        if (station == null)
        {
            Console.WriteLine($"Station with name '{stationName}' not found in database");
            return null;
        }

        return station.Id;
    }

    private long? GetStationIdByCode(string stationCode, string? stationName = null)
    {
        var station = stations.FirstOrDefault(s => s.Code == stationCode);

        if (station == null)
        {
            // Создаем новую станцию, если не нашли по коду
            if (!string.IsNullOrWhiteSpace(stationName))
            {
                var newStation = new Station
                {
                    Code = stationCode,
                    Name = stationName
                };
                
                dbContext.Stations!.Add(newStation);
                dbContext.SaveChanges();
                stations.Add(newStation);
                
                Console.WriteLine($"Created new station: '{stationName}' with code '{stationCode}', ID: {newStation.Id}");
                return newStation.Id;
            }
            
            Console.WriteLine($"Warning: Station with code '{stationCode}' not found in database and no name provided");
            return null;
        }

        return station.Id;
    }
    
    public void ParseRouteItem(RouteItem item)
    {
        if (item == null) return;
        
        if (!string.IsNullOrWhiteSpace(item.Arrival) && item.Arrival?.Trim() != "-")
            item.ArrivalTime = ParseTime(item.Arrival);
        item.Stop = item.Stop?.Trim('*');
        if (!string.IsNullOrWhiteSpace(item.Stop) && item.Stop?.Trim() != "-")
            item.StopMinutes = ParseStopMinutes(item.Stop);
        if (!string.IsNullOrWhiteSpace(item.Departure) && item.Departure?.Trim() != "-")
            item.DepartureTime = ParseTime(item.Departure);
        item.DistanceKm = ParseDistance(item.Distance);
    }
    
    public void ParseRouteItems(IEnumerable<RouteItem> items)
    {
        TimeSpan? lastTime = null;
        int dayOffset = 0;
        
        foreach (var item in items)
        {
            ParseRouteItem(item);

            // Устанавливаем StationId по коду станции
            if (!string.IsNullOrWhiteSpace(item.StationCode))
            {
                item.StationCode = item.StationCode.TrimStart('0');
                item.StationId = GetStationIdByCode(item.StationCode, item.StationName);
            }
            else if (!string.IsNullOrWhiteSpace(item.StationName))
            {
                // Если кода нет, но есть имя - ищем по имени
                item.StationId = GetStationIdByName(item.StationName);
                
                // Если не найдено - создаем новую станцию по имени
                if (item.StationId == null)
                {
                    var newStation = new Station
                    {
                        Name = item.StationName
                    };
                    
                    dbContext.Stations!.Add(newStation);
                    dbContext.SaveChanges();
                    stations.Add(newStation);
                    
                    Console.WriteLine($"Created new station by name: '{item.StationName}', ID: {newStation.Id}");
                    item.StationId = newStation.Id;
                }
            }
            else
            {
                throw new Exception($"Both StationCode and StationName are null or empty");
            }

            if (item.StationId == 1440)
            {
                
            }

            // Берем основное время: DepartureTime, если нет - ArrivalTime
            var currentTime = item.DepartureTime ?? item.ArrivalTime;
            
            if (!currentTime.HasValue)
            {
                continue;
            }

            // Применяем offset к обоим временам
            if (dayOffset > 0)
            {
                if (item.ArrivalTime.HasValue)
                {
                    item.ArrivalTime = new TimeSpan(dayOffset, 
                        item.ArrivalTime.Value.Hours,
                        item.ArrivalTime.Value.Minutes,
                        item.ArrivalTime.Value.Seconds);
                }

                if (item.DepartureTime.HasValue)
                {
                    item.DepartureTime = new TimeSpan(dayOffset,
                        item.DepartureTime.Value.Hours,
                        item.DepartureTime.Value.Minutes,
                        item.DepartureTime.Value.Seconds);
                }
            }

            // Проверяем переход на следующий день
            if (item.DepartureTime.HasValue && item.ArrivalTime.HasValue &&
                item.DepartureTime.Value < item.ArrivalTime.Value)
            {
                dayOffset++;
                item.DepartureTime = new TimeSpan(dayOffset,
                    item.DepartureTime.Value.Hours,
                    item.DepartureTime.Value.Minutes,
                    item.DepartureTime.Value.Seconds);
            }
            else if (item.DepartureTime.HasValue && item.ArrivalTime.HasValue && lastTime.HasValue &&
                (item.DepartureTime.Value < lastTime.Value || item.ArrivalTime.Value < lastTime.Value))
            {
                dayOffset++;
                item.ArrivalTime = new TimeSpan(dayOffset,
                    item.ArrivalTime.Value.Hours,
                    item.ArrivalTime.Value.Minutes,
                    item.ArrivalTime.Value.Seconds);
                item.DepartureTime = new TimeSpan(dayOffset,
                    item.DepartureTime.Value.Hours,
                    item.DepartureTime.Value.Minutes,
                    item.DepartureTime.Value.Seconds);
            }
            else if (item.DepartureTime.HasValue && lastTime.HasValue &&
                     item.DepartureTime.Value < lastTime.Value)
            {
                dayOffset++;
                item.DepartureTime = new TimeSpan(dayOffset,
                    item.DepartureTime.Value.Hours,
                    item.DepartureTime.Value.Minutes,
                    item.DepartureTime.Value.Seconds);
            }
            else if (item.ArrivalTime.HasValue && lastTime.HasValue &&
                     item.ArrivalTime.Value < lastTime.Value)
            {
                dayOffset++;
                item.ArrivalTime = new TimeSpan(dayOffset,
                    item.ArrivalTime.Value.Hours,
                    item.ArrivalTime.Value.Minutes,
                    item.ArrivalTime.Value.Seconds);
            }
            
            item.Day = dayOffset;

            // Обновляем последнее время
            lastTime = item.DepartureTime ?? item.ArrivalTime;
        }
    }
    
    private static TimeSpan? ParseTime(string? timeStr)
    {
        if (string.IsNullOrWhiteSpace(timeStr))
            return null;
        
        timeStr = timeStr.Trim();
        
        // Формат: "HH.MM" или "HH:MM"
        var parts = timeStr.Replace('.', ':').Split(':');
        if (parts.Length == 2 && 
            int.TryParse(parts[0], out int hours) && 
            int.TryParse(parts[1], out int minutes))
        {
            return new TimeSpan(hours, minutes, 0);
        }
        
        return null;
    }
    
    private static int? ParseStopMinutes(string? stopStr)
    {
        if (string.IsNullOrWhiteSpace(stopStr))
            return null;
        
        stopStr = stopStr.Trim();
        
        if (int.TryParse(stopStr, out int minutes))
        {
            return minutes;
        }
        
        return null;
    }
    
    private static double? ParseDistance(string? distanceStr)
    {
        if (string.IsNullOrWhiteSpace(distanceStr))
            return null;
        
        distanceStr = distanceStr.Trim();
        
        // Убираем возможные единицы измерения
        distanceStr = distanceStr
            .Replace("км", "", StringComparison.OrdinalIgnoreCase)
            .Replace("km", "", StringComparison.OrdinalIgnoreCase)
            .Trim();
        
        // Заменяем запятую на точку для парсинга
        distanceStr = distanceStr.Replace(',', '.');
        
        if (double.TryParse(distanceStr, out double distance))
        {
            return distance;
        }
        
        return null;
    }
}

