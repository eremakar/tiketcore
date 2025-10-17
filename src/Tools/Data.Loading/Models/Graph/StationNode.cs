namespace Data.Loading.Models.Graph;

/// <summary>
/// Вершина графа - станция
/// </summary>
public class StationNode
{
    public long StationId { get; set; }
    public string StationCode { get; set; } = string.Empty;
    public string StationName { get; set; } = string.Empty;
    
    /// <summary>
    /// Исходящие рёбра (связи с другими станциями)
    /// </summary>
    public List<StationEdge> OutgoingEdges { get; set; } = new();
    
    /// <summary>
    /// Входящие рёбра (связи от других станций)
    /// </summary>
    public List<StationEdge> IncomingEdges { get; set; } = new();

    public override string ToString()
    {
        return $"{StationName} (ID: {StationId}, Code: {StationCode})";
    }

    public override bool Equals(object? obj)
    {
        if (obj is StationNode other)
        {
            return StationId == other.StationId;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return StationId.GetHashCode();
    }
}

