namespace Data.Loading.Models.Graph;

/// <summary>
/// Граф железнодорожных станций
/// </summary>
public class StationGraph
{
    /// <summary>
    /// Все вершины графа (станции), индексированные по StationId
    /// </summary>
    public Dictionary<long, StationNode> Nodes { get; set; } = new();
    
    /// <summary>
    /// Все рёбра графа (связи между станциями)
    /// </summary>
    public List<StationEdge> Edges { get; set; } = new();

    /// <summary>
    /// Количество станций в графе
    /// </summary>
    public int StationCount => Nodes.Count;
    
    /// <summary>
    /// Количество связей (рёбер) в графе
    /// </summary>
    public int EdgeCount => Edges.Count;

    /// <summary>
    /// Получить вершину (станцию) по ID
    /// </summary>
    public StationNode? GetNode(long stationId)
    {
        return Nodes.TryGetValue(stationId, out var node) ? node : null;
    }

    /// <summary>
    /// Получить все исходящие рёбра для станции
    /// </summary>
    public IEnumerable<StationEdge> GetOutgoingEdges(long stationId)
    {
        var node = GetNode(stationId);
        return node?.OutgoingEdges ?? Enumerable.Empty<StationEdge>();
    }

    /// <summary>
    /// Получить все входящие рёбра для станции
    /// </summary>
    public IEnumerable<StationEdge> GetIncomingEdges(long stationId)
    {
        var node = GetNode(stationId);
        return node?.IncomingEdges ?? Enumerable.Empty<StationEdge>();
    }

    /// <summary>
    /// Получить все соседние станции для заданной станции
    /// </summary>
    public IEnumerable<StationNode> GetNeighbors(long stationId)
    {
        var node = GetNode(stationId);
        if (node == null)
            return Enumerable.Empty<StationNode>();

        return node.OutgoingEdges.Select(e => e.ToStation)
            .Union(node.IncomingEdges.Select(e => e.FromStation));
    }

    public override string ToString()
    {
        return $"StationGraph: {StationCount} станций, {EdgeCount} связей";
    }
}

