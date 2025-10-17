using Data.Loading.Models.Graph;

namespace Data.Loading.Models;

/// <summary>
/// Результат загрузки и обработки данных о поездах
/// </summary>
public class TrainsLoadResult
{
    public List<RouteData> Routes { get; set; } = new();
    public StationGraph Graph { get; set; } = new();
    public List<StationNode> HubStations { get; set; } = new();
}

