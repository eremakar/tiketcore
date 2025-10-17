namespace Data.Loading.Models;

public class TrainWagon
{
    public string Number { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public List<SeatCount> SeatCounts { get; set; } = new();
}

