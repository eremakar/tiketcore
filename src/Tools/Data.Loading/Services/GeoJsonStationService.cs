using System.Text.Json;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using QuickGraph;
using QuickGraph.Algorithms;

namespace Data.Loading.Services;

/// <summary>
/// Сервис для работы с геоданными станций из GeoJSON и расчета расстояний по рельсам
/// </summary>
public class GeoJsonStationService
{
    private FeatureCollection? featureCollection;
    private Dictionary<string, Position> stations = new();
    private UndirectedGraph<RailNode, TaggedEdge<RailNode, double>>? railwayGraph;
    
    /// <summary>
    /// Загрузить станции из GeoJSON файла
    /// </summary>
    public async Task LoadFromFileAsync(string filePath)
    {
        Console.WriteLine($"Загрузка GeoJSON из {filePath}...");
        
        var json = await File.ReadAllTextAsync(filePath);
        featureCollection = JsonSerializer.Deserialize<FeatureCollection>(json);
        
        if (featureCollection?.Features == null)
        {
            Console.WriteLine("Ошибка: неверный формат GeoJSON");
            return;
        }
        
        // Извлекаем станции (Point)
        ExtractStations();
        
        // Строим граф железнодорожных линий (LineString)
        BuildRailwayGraph();
        
        Console.WriteLine($"Загружено станций: {stations.Count}");
        Console.WriteLine($"Построено узлов графа: {railwayGraph?.VertexCount ?? 0}");
        Console.WriteLine($"Построено рёбер графа: {railwayGraph?.EdgeCount ?? 0}");
    }
    
    /// <summary>
    /// Извлечь станции из GeoJSON
    /// </summary>
    private void ExtractStations()
    {
        stations.Clear();
        
        if (featureCollection?.Features == null) return;
        
        foreach (var feature in featureCollection.Features)
        {
            // Проверяем геометрию Point
            if (feature.Geometry is Point point)
            {
                string? stationName = null;
                
                // Ищем имя станции в properties
                if (feature.Properties != null)
                {
                    // Попробуем различные ключи
                    if (feature.Properties.TryGetValue("name", out var nameObj))
                        stationName = nameObj?.ToString();
                    else if (feature.Properties.TryGetValue("Name", out nameObj))
                        stationName = nameObj?.ToString();
                    
                    // Если имя не найдено, попробуем в relations
                    if (string.IsNullOrEmpty(stationName) && feature.Properties.ContainsKey("@relations"))
                    {
                        stationName = ExtractNameFromRelations(feature.Properties["@relations"]);
                    }
                }
                
                if (!string.IsNullOrEmpty(stationName) && point.Coordinates != null)
                {
                    stations[stationName] = point.Coordinates;
                }
            }
            // Проверяем другие типы геометрии
            else if (feature.Geometry is Polygon || feature.Geometry is MultiPolygon)
            {
                string? stationName = null;
                
                if (feature.Properties != null && feature.Properties.ContainsKey("@relations"))
                {
                    stationName = ExtractNameFromRelations(feature.Properties["@relations"]);
                }
                
                if (!string.IsNullOrEmpty(stationName))
                {
                    var centroid = GetCentroid(feature.Geometry);
                    if (centroid != null)
                    {
                        stations[stationName] = centroid.Value;
                    }
                }
            }
        }
    }
    
    /// <summary>
    /// Извлечь имя станции из relations
    /// </summary>
    private string? ExtractNameFromRelations(object? relationsObj)
    {
        try
        {
            if (relationsObj is JsonElement relElement && relElement.ValueKind == JsonValueKind.Array)
            {
                foreach (var rel in relElement.EnumerateArray())
                {
                    if (rel.TryGetProperty("reltags", out var relTags))
                    {
                        if (relTags.TryGetProperty("name", out var nameElement))
                        {
                            return nameElement.GetString();
                        }
                    }
                }
            }
        }
        catch { }
        
        return null;
    }
    
    /// <summary>
    /// Получить центроид геометрии
    /// </summary>
    private Position? GetCentroid(IGeometryObject geometry)
    {
        try
        {
            if (geometry is Polygon polygon && polygon.Coordinates?.Count > 0)
            {
                var coords = polygon.Coordinates[0].Coordinates;
                if (coords.Count > 0)
                {
                    double avgLon = coords.Average(c => c.Longitude);
                    double avgLat = coords.Average(c => c.Latitude);
                    return new Position(avgLat, avgLon);
                }
            }
        }
        catch { }
        
        return null;
    }
    
    /// <summary>
    /// Построить граф железнодорожных линий
    /// </summary>
    private void BuildRailwayGraph()
    {
        railwayGraph = new UndirectedGraph<RailNode, TaggedEdge<RailNode, double>>(allowParallelEdges: false);
        
        if (featureCollection?.Features == null) return;
        
        foreach (var feature in featureCollection.Features)
        {
            if (feature.Geometry is LineString lineString && lineString.Coordinates?.Count > 0)
            {
                var coords = lineString.Coordinates;
                for (int i = 0; i < coords.Count - 1; i++)
                {
                    var n1 = new RailNode(coords[i].Longitude, coords[i].Latitude);
                    var n2 = new RailNode(coords[i + 1].Longitude, coords[i + 1].Latitude);
                    double dist = Haversine(n1.Longitude, n1.Latitude, n2.Longitude, n2.Latitude);
                    
                    railwayGraph.AddVerticesAndEdge(new TaggedEdge<RailNode, double>(n1, n2, dist));
                }
            }
            else if (feature.Geometry is MultiLineString multiLineString && multiLineString.Coordinates?.Count > 0)
            {
                foreach (var lineString in multiLineString.Coordinates)
                {
                    if (lineString.Coordinates?.Count > 0)
                    {
                        var coords = lineString.Coordinates;
                        for (int i = 0; i < coords.Count - 1; i++)
                        {
                            var n1 = new RailNode(coords[i].Longitude, coords[i].Latitude);
                            var n2 = new RailNode(coords[i + 1].Longitude, coords[i + 1].Latitude);
                            double dist = Haversine(n1.Longitude, n1.Latitude, n2.Longitude, n2.Latitude);
                            
                            railwayGraph.AddVerticesAndEdge(new TaggedEdge<RailNode, double>(n1, n2, dist));
                        }
                    }
                }
            }
        }
    }
    
    /// <summary>
    /// Найти станцию по имени
    /// </summary>
    public Position? FindStationByName(string stationName)
    {
        // Точное совпадение
        if (stations.TryGetValue(stationName, out var position))
            return position;
        
        // Точное совпадение без учета регистра
        var exactMatch = stations.FirstOrDefault(s => 
            string.Equals(s.Key, stationName, StringComparison.OrdinalIgnoreCase));
        if (exactMatch.Key != null)
            return exactMatch.Value;
        
        // Частичное совпадение
        var partialMatch = stations.FirstOrDefault(s => 
            s.Key.Contains(stationName, StringComparison.OrdinalIgnoreCase) ||
            stationName.Contains(s.Key, StringComparison.OrdinalIgnoreCase));
        
        return partialMatch.Key != null ? partialMatch.Value : null;
    }
    
    /// <summary>
    /// Вычислить расстояние между двумя станциями по рельсам (кратчайший путь по графу)
    /// </summary>
    public double CalculateRailwayDistance(string station1Name, string station2Name)
    {
        var pos1 = FindStationByName(station1Name);
        var pos2 = FindStationByName(station2Name);
        
        if (pos1 == null)
            throw new Exception($"Станция '{station1Name}' не найдена в GeoJSON");
        
        if (pos2 == null)
            throw new Exception($"Станция '{station2Name}' не найдена в GeoJSON");
        
        if (railwayGraph == null || railwayGraph.VertexCount == 0)
            throw new Exception("Граф железнодорожных линий не построен");
        
        // Находим ближайшие узлы графа к станциям
        var node1 = FindNearestNode(pos1);
        var node2 = FindNearestNode(pos2);
        
        Console.WriteLine($"\n=== Расчет расстояния по рельсам ===");
        Console.WriteLine($"Станция 1: {station1Name} ({pos1.Latitude:F6}, {pos1.Longitude:F6})");
        Console.WriteLine($"Ближайший узел графа 1: ({node1.Latitude:F6}, {node1.Longitude:F6})");
        Console.WriteLine($"Станция 2: {station2Name} ({pos2.Latitude:F6}, {pos2.Longitude:F6})");
        Console.WriteLine($"Ближайший узел графа 2: ({node2.Latitude:F6}, {node2.Longitude:F6})");
        
        // Ищем кратчайший путь по графу
        var tryGetPath = railwayGraph.ShortestPathsDijkstra(e => e.Tag, node1);
        
        if (tryGetPath(node2, out var path))
        {
            double totalKm = path.Sum(e => e.Tag);
            Console.WriteLine($"Количество сегментов пути: {path.Count()}");
            Console.WriteLine($"Расстояние по рельсам: {totalKm:F2} км");
            return totalKm;
        }
        else
        {
            throw new Exception($"Путь между станциями '{station1Name}' и '{station2Name}' не найден в графе железнодорожных линий");
        }
    }
    
    /// <summary>
    /// Найти ближайший узел графа к заданной позиции
    /// </summary>
    private RailNode FindNearestNode(Position target)
    {
        if (railwayGraph == null || railwayGraph.VertexCount == 0)
            throw new Exception("Граф пуст");
        
        return railwayGraph.Vertices
            .OrderBy(v => Haversine(v.Longitude, v.Latitude, target.Longitude, target.Latitude))
            .First();
    }
    
    /// <summary>
    /// Формула Гаверсинуса для вычисления расстояния между двумя точками на сфере (в км)
    /// </summary>
    private double Haversine(double lon1, double lat1, double lon2, double lat2)
    {
        const double R = 6371.0; // Радиус Земли в километрах
        
        double dLat = ToRadians(lat2 - lat1);
        double dLon = ToRadians(lon2 - lon1);
        double lat1Rad = ToRadians(lat1);
        double lat2Rad = ToRadians(lat2);
        
        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        
        return R * c;
    }
    
    private double ToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
    }
    
    /// <summary>
    /// Получить список всех станций
    /// </summary>
    public Dictionary<string, Position> GetAllStations() => stations;
}

/// <summary>
/// Узел железнодорожного графа
/// </summary>
public record RailNode(double Longitude, double Latitude);

