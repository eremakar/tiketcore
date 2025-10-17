namespace Data.Loading.Models;

public class Train
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string StartStation { get; set; } = string.Empty;
    public long? StartStationId { get; set; }
    public string EndStation { get; set; } = string.Empty;
    public long? EndStationId { get; set; }
    public List<RouteItem> RouteItems { get; set; } = new();
    public List<TrainWagon> Wagons { get; set; } = new();
}

