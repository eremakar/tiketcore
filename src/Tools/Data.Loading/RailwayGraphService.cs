using Data.Loading.Models;
using Data.Loading.Models.Graph;
using Microsoft.EntityFrameworkCore;
using Ticketing.Data.TicketDb.DatabaseContext;

namespace Data.Loading;

/// <summary>
/// Сервис для построения графа железнодорожной сети на основе маршрутов поездов
/// </summary>
public class RailwayGraphService
{
    private readonly TicketDbContext dbContext;

    public RailwayGraphService(TicketDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <summary>
    /// Построить граф станций на основе всех маршрутов поездов
    /// </summary>
    public StationGraph BuildGraph(List<RouteData> routes)
    {
        var graph = new StationGraph();
        
        Console.WriteLine("Начало построения графа станций...");

        // Подсчёт статистики
        int totalTrains = 0;
        int processedTrains = 0;
        int skippedTrains = 0;

        foreach (var route in routes)
        {
            totalTrains += 2; // каждый маршрут содержит 2 поезда
            
            ProcessTrain(graph, route.Train1);
            ProcessTrain(graph, route.Train2);
        }

        // Вывод статистики
        Console.WriteLine($"\n=== Статистика построения графа ===");
        Console.WriteLine($"Обработано маршрутов: {routes.Count}");
        Console.WriteLine($"Обработано поездов: {totalTrains}");
        Console.WriteLine($"Станций в графе: {graph.StationCount}");
        Console.WriteLine($"Связей в графе: {graph.EdgeCount}");
        Console.WriteLine($"Среднее количество поездов на связь: {(graph.EdgeCount > 0 ? graph.Edges.Average(e => e.TrainCount) : 0):F2}");

        PrintGraphStatistics(graph);

        return graph;
    }

    /// <summary>
    /// Обработать один поезд и добавить его маршрут в граф
    /// </summary>
    private void ProcessTrain(StationGraph graph, Train train)
    {
        if (train.RouteItems == null || train.RouteItems.Count < 2)
        {
            Console.WriteLine($"Поезд {train.Name} пропущен: недостаточно станций в маршруте ({train.RouteItems?.Count ?? 0})");
            return;
        }

        if (train.RouteItems.Any(ri => ri.StationId == null || ri.StationId <= 0))
        {
            var routeItem = train.RouteItems.FirstOrDefault(ri => ri.StationId == null || ri.StationId <= 0);
            Console.WriteLine($"Поезд {train.Name} пропущен: маршрут содержит станции с некорректным StationId");
            throw new Exception($"Некорректные StationId в маршруте поезда {train.Name}");
        }

        // Отфильтровываем станции с валидным StationId
        var validStations = train.RouteItems
            .Where(ri => ri.StationId.HasValue && ri.StationId.Value > 0)
            .ToList();

        if (validStations.Count < 2)
        {
            Console.WriteLine($"Поезд {train.Name} пропущен: недостаточно валидных станций ({validStations.Count} из {train.RouteItems.Count})");
            return;
        }

        // Проходим по всем последовательным парам станций
        for (int i = 0; i < validStations.Count - 1; i++)
        {
            var currentItem = validStations[i];
            var nextItem = validStations[i + 1];

            // Добавляем или получаем вершины (станции)
            var fromNode = GetOrCreateNode(graph, currentItem);
            var toNode = GetOrCreateNode(graph, nextItem);

            // Добавляем или обновляем ребро
            AddOrUpdateEdge(graph, fromNode, toNode, nextItem.DistanceKm, train.Name);
        }
    }

    /// <summary>
    /// Получить существующую вершину или создать новую
    /// </summary>
    private StationNode GetOrCreateNode(StationGraph graph, RouteItem routeItem)
    {
        var stationId = routeItem.StationId!.Value;

        if (graph.Nodes.TryGetValue(stationId, out var existingNode))
        {
            return existingNode;
        }

        // Создаём новую вершину
        var newNode = new StationNode
        {
            StationId = stationId,
            StationCode = routeItem.StationCode ?? string.Empty,
            StationName = routeItem.StationName ?? string.Empty
        };

        graph.Nodes[stationId] = newNode;
        return newNode;
    }

    /// <summary>
    /// Добавить новое ребро или обновить существующее
    /// </summary>
    private void AddOrUpdateEdge(StationGraph graph, StationNode fromNode, StationNode toNode, double? distanceKm, string trainName)
    {
        // Ищем существующее ребро
        var existingEdge = fromNode.OutgoingEdges
            .FirstOrDefault(e => e.ToStation.StationId == toNode.StationId);

        if (existingEdge != null)
        {
            // Обновляем существующее ребро
            existingEdge.TrainNames.Add(trainName);
            
            // Обновляем расстояние если оно не было задано
            if (!existingEdge.DistanceKm.HasValue && distanceKm.HasValue)
            {
                existingEdge.DistanceKm = distanceKm;
            }
        }
        else
        {
            // Создаём новое ребро
            var newEdge = new StationEdge
            {
                FromStation = fromNode,
                ToStation = toNode,
                DistanceKm = distanceKm,
                TrainNames = new HashSet<string> { trainName }
            };

            // Добавляем ребро в граф и в списки исходящих/входящих рёбер
            graph.Edges.Add(newEdge);
            fromNode.OutgoingEdges.Add(newEdge);
            toNode.IncomingEdges.Add(newEdge);
        }
    }

    /// <summary>
    /// Вывести детальную статистику по графу
    /// </summary>
    private void PrintGraphStatistics(StationGraph graph)
    {
        Console.WriteLine($"\n=== Детальная статистика графа ===");

        // Топ-10 станций с наибольшим количеством связей
        var topStationsByConnections = graph.Nodes.Values
            .OrderByDescending(n => n.OutgoingEdges.Count + n.IncomingEdges.Count)
            .Take(10)
            .ToList();

        Console.WriteLine($"\nТоп-10 станций с наибольшим количеством связей:");
        foreach (var node in topStationsByConnections)
        {
            var totalConnections = node.OutgoingEdges.Count + node.IncomingEdges.Count;
            Console.WriteLine($"  {node.StationName}: {totalConnections} связей (исх: {node.OutgoingEdges.Count}, вх: {node.IncomingEdges.Count})");
        }

        // Топ-10 связей с наибольшим количеством поездов
        var topEdgesByTrains = graph.Edges
            .OrderByDescending(e => e.TrainCount)
            .Take(10)
            .ToList();

        Console.WriteLine($"\nТоп-10 связей с наибольшим количеством поездов:");
        foreach (var edge in topEdgesByTrains)
        {
            Console.WriteLine($"  {edge.FromStation.StationName} → {edge.ToStation.StationName}: {edge.TrainCount} поездов");
        }

        // Статистика по степени связности
        var degrees = graph.Nodes.Values
            .Select(n => n.OutgoingEdges.Count + n.IncomingEdges.Count)
            .ToList();

        if (degrees.Any())
        {
            Console.WriteLine($"\nСтатистика связности станций:");
            Console.WriteLine($"  Минимальная степень: {degrees.Min()}");
            Console.WriteLine($"  Максимальная степень: {degrees.Max()}");
            Console.WriteLine($"  Средняя степень: {degrees.Average():F2}");
            Console.WriteLine($"  Медианная степень: {GetMedian(degrees):F2}");
        }

        // Статистика по количеству поездов на связь
        var trainCounts = graph.Edges.Select(e => e.TrainCount).ToList();
        
        if (trainCounts.Any())
        {
            Console.WriteLine($"\nСтатистика по количеству поездов на связь:");
            Console.WriteLine($"  Минимум: {trainCounts.Min()}");
            Console.WriteLine($"  Максимум: {trainCounts.Max()}");
            Console.WriteLine($"  Среднее: {trainCounts.Average():F2}");
            Console.WriteLine($"  Медиана: {GetMedian(trainCounts):F2}");
        }
    }

    /// <summary>
    /// Вычислить медиану для списка чисел
    /// </summary>
    private double GetMedian(List<int> values)
    {
        if (values.Count == 0)
            return 0;

        var sorted = values.OrderBy(v => v).ToList();
        int mid = sorted.Count / 2;

        if (sorted.Count % 2 == 0)
        {
            return (sorted[mid - 1] + sorted[mid]) / 2.0;
        }
        else
        {
            return sorted[mid];
        }
    }

    /// <summary>
    /// Получить список узловых станций (станции с более чем 2 исходящими рёбрами)
    /// </summary>
    public List<StationNode> GetHubStations(StationGraph graph)
    {
        var hubStations = graph.Nodes.Values
            .Where(node => node.OutgoingEdges.Count > 2)
            .OrderByDescending(node => node.OutgoingEdges.Count)
            .ToList();

        Console.WriteLine($"\n=== Узловые станции (с более чем 2 исходящими связями) ===");
        Console.WriteLine($"Найдено узловых станций: {hubStations.Count}");
        
        foreach (var station in hubStations)
        {
            Console.WriteLine($"  {station.StationName} ({station.StationCode}): {station.OutgoingEdges.Count} исходящих, {station.IncomingEdges.Count} входящих");
        }

        return hubStations;
    }

    /// <summary>
    /// Экспортировать граф в файл для визуализации (например, в формате DOT)
    /// </summary>
    public void ExportToDot(StationGraph graph, string outputPath)
    {
        using var writer = new StreamWriter(outputPath);
        
        writer.WriteLine("digraph RailwayNetwork {");
        writer.WriteLine("  rankdir=LR;");
        writer.WriteLine("  node [shape=box];");
        writer.WriteLine();

        // Вершины
        foreach (var node in graph.Nodes.Values)
        {
            writer.WriteLine($"  \"{node.StationId}\" [label=\"{node.StationName}\\n{node.StationCode}\"];");
        }

        writer.WriteLine();

        // Рёбра
        foreach (var edge in graph.Edges)
        {
            var label = $"{edge.TrainCount} поездов";
            if (edge.DistanceKm.HasValue)
            {
                label += $"\\n{edge.DistanceKm:F1} км";
            }
            
            writer.WriteLine($"  \"{edge.FromStation.StationId}\" -> \"{edge.ToStation.StationId}\" [label=\"{label}\"];");
        }

        writer.WriteLine("}");
        
        Console.WriteLine($"\nГраф экспортирован в файл: {outputPath}");
        Console.WriteLine("Для визуализации используйте Graphviz: dot -Tpng output.dot -o output.png");
    }

    /// <summary>
    /// Найти все пары станций с аномальным расстоянием (разница > 30 км)
    /// </summary>
    public void CheckDistanceAnomalies(List<RouteData> routes, StationGraph graph, double thresholdKm = 50)
    {
        Console.WriteLine($"\n=== Проверка аномалий в расстояниях (порог: {thresholdKm} км) ===");
        
        // Определяем hub станции (станции с более чем 2 исходящими рёбрами)
        var hubStationIds = graph.Nodes.Values
            .Where(node => node.OutgoingEdges.Count > 2)
            .Select(node => node.StationId)
            .ToHashSet();

        Console.WriteLine($"Узловых станций: {hubStationIds.Count}");
        Console.WriteLine();

        // Собираем уникальные аномалии
        var uniqueAnomalies = new Dictionary<string, AnomalyInfo>();

        foreach (var route in routes)
        {
            CollectTrainDistanceAnomalies(route.Train1, hubStationIds, thresholdKm, uniqueAnomalies);
            CollectTrainDistanceAnomalies(route.Train2, hubStationIds, thresholdKm, uniqueAnomalies);
        }

        // Выводим уникальные аномалии
        int anomaliesWithHub = 0;
        foreach (var anomaly in uniqueAnomalies.Values.OrderByDescending(a => a.DistanceDiff))
        {
            bool currentIsHub = anomaly.CurrentStationId.HasValue && hubStationIds.Contains(anomaly.CurrentStationId.Value);
            bool nextIsHub = anomaly.NextStationId.HasValue && hubStationIds.Contains(anomaly.NextStationId.Value);
            bool hasHub = currentIsHub || nextIsHub;

            if (hasHub)
                anomaliesWithHub++;

            string hubMarker = hasHub ? " [HUB]" : "";
            string currentHubMarker = currentIsHub ? " [HUB]" : "";
            string nextHubMarker = nextIsHub ? " [HUB]" : "";

            Console.WriteLine($"{anomaly.CurrentStationName}{currentHubMarker} ({anomaly.CurrentDistanceKm:F1} км) → " +
                            $"{anomaly.NextStationName}{nextHubMarker} ({anomaly.NextDistanceKm:F1} км) | " +
                            $"Разница: {anomaly.DistanceDiff:F1} км{hubMarker}");
            
            // Выводим информацию о поездах
            foreach (var trainInfo in anomaly.Trains)
            {
                Console.WriteLine($"  └─ Поезд {trainInfo.TrainNumber}: {trainInfo.RouteFrom} → {trainInfo.RouteTo}");
            }
        }

        Console.WriteLine($"\nВсего уникальных аномалий: {uniqueAnomalies.Count}");
        Console.WriteLine($"Из них с участием узловых станций: {anomaliesWithHub}");
    }

    /// <summary>
    /// Собрать аномалии расстояний для одного поезда
    /// </summary>
    private void CollectTrainDistanceAnomalies(Train train, HashSet<long> hubStationIds, double thresholdKm, Dictionary<string, AnomalyInfo> uniqueAnomalies)
    {
        if (train.RouteItems == null || train.RouteItems.Count < 2)
            return;

        for (int i = 0; i < train.RouteItems.Count - 1; i++)
        {
            var currentItem = train.RouteItems[i];
            var nextItem = train.RouteItems[i + 1];

            if (!currentItem.DistanceKm.HasValue || !nextItem.DistanceKm.HasValue)
                continue;

            double distanceDiff = Math.Abs(nextItem.DistanceKm.Value - currentItem.DistanceKm.Value);

            if (distanceDiff > thresholdKm)
            {
                // Создаём уникальный ключ для пары станций (независимо от направления)
                var id1 = currentItem.StationId ?? 0;
                var id2 = nextItem.StationId ?? 0;
                string key = id1 < id2 ? $"{id1}_{id2}" : $"{id2}_{id1}";
                
                if (!uniqueAnomalies.ContainsKey(key))
                {
                    uniqueAnomalies[key] = new AnomalyInfo
                    {
                        CurrentStationId = currentItem.StationId,
                        CurrentStationName = currentItem.StationName ?? "",
                        CurrentDistanceKm = currentItem.DistanceKm.Value,
                        NextStationId = nextItem.StationId,
                        NextStationName = nextItem.StationName ?? "",
                        NextDistanceKm = nextItem.DistanceKm.Value,
                        DistanceDiff = distanceDiff,
                        Trains = new List<TrainRouteInfo>()
                    };
                }
                
                // Добавляем информацию о поезде
                var routeFrom = train.RouteItems.FirstOrDefault()?.StationName ?? "?";
                var routeTo = train.RouteItems.LastOrDefault()?.StationName ?? "?";
                
                uniqueAnomalies[key].Trains.Add(new TrainRouteInfo
                {
                    TrainNumber = train.Name ?? "?",
                    RouteFrom = routeFrom,
                    RouteTo = routeTo
                });
            }
        }
    }

    /// <summary>
    /// Информация об аномалии расстояния
    /// </summary>
    private class AnomalyInfo
    {
        public long? CurrentStationId { get; set; }
        public string CurrentStationName { get; set; } = "";
        public double CurrentDistanceKm { get; set; }
        public long? NextStationId { get; set; }
        public string NextStationName { get; set; } = "";
        public double NextDistanceKm { get; set; }
        public double DistanceDiff { get; set; }
        public List<TrainRouteInfo> Trains { get; set; } = new();
    }
    
    /// <summary>
    /// Информация о поезде и его маршруте
    /// </summary>
    private class TrainRouteInfo
    {
        public string TrainNumber { get; set; } = "";
        public string RouteFrom { get; set; } = "";
        public string RouteTo { get; set; } = "";
    }
}

