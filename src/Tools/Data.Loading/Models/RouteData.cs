namespace Data.Loading.Models;

public class RouteData
{
    public int? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Train Train1 { get; set; } = new();
    public Train Train2 { get; set; } = new();
}

