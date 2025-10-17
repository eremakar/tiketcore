namespace Data.Loading.Models;

public class RouteItem
{
    public string? Arrival { get; set; }
    public TimeSpan? ArrivalTime { get; set; }
    public string? Stop { get; set; }
    public int? StopMinutes { get; set; }
    public string? Departure { get; set; }
    public TimeSpan? DepartureTime { get; set; }
    public string? StationName { get; set; }
    public long? StationId { get; set; }
    public string? Distance { get; set; }
    public double? DistanceKm { get; set; }
    public string? StationCode { get; set; }
    public int? Day { get; set; }
    
    public override string ToString()
    {
        return $"{StationName} (Код: {StationCode}, Расст: {Distance}км) [Приб: {Arrival}, Сто: {Stop}, Отпр: {Departure}]";
    }
}

