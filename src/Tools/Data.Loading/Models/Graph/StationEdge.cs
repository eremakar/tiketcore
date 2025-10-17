namespace Data.Loading.Models.Graph;

/// <summary>
/// Ребро графа - связь между двумя станциями в маршруте
/// </summary>
public class StationEdge
{
    public StationNode FromStation { get; set; } = null!;
    public StationNode ToStation { get; set; } = null!;
    
    /// <summary>
    /// Расстояние между станциями (км)
    /// </summary>
    public double? DistanceKm { get; set; }
    
    /// <summary>
    /// Поезда, которые проходят по этому ребру (названия поездов)
    /// </summary>
    public HashSet<string> TrainNames { get; set; } = new();
    
    /// <summary>
    /// Количество поездов, проходящих по этому ребру
    /// </summary>
    public int TrainCount => TrainNames.Count;

    public override string ToString()
    {
        return $"{FromStation.StationName} → {ToStation.StationName} (Поездов: {TrainCount}, Расст: {DistanceKm}км)";
    }

    public override bool Equals(object? obj)
    {
        if (obj is StationEdge other)
        {
            return FromStation.Equals(other.FromStation) && ToStation.Equals(other.ToStation);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(FromStation.GetHashCode(), ToStation.GetHashCode());
    }
}

