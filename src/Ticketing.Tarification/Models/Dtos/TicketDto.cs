
namespace Ticketing.Tarifications.Models.Dtos
{
    /// <summary>
    /// Билет
    /// </summary>
    public partial class TicketDto
    {
        public long Id { get; set; }
        public string? Number { get; set; }
        public DateTime Date { get; set; }
        public bool IsSeat { get; set; }
        public double Price { get; set; }
        public int State { get; set; }
        public int Type { get; set; }
        public string? Total { get; set; }
        public long? FromId { get; set; }
        public long? ToId { get; set; }
        public long? TrainId { get; set; }
        public long? WagonId { get; set; }
        public long? SeatId { get; set; }
        public long? TrainScheduleId { get; set; }

        public RouteStationDto? From { get; set; }
        public RouteStationDto? To { get; set; }
        public TrainDto? Train { get; set; }
        public TrainWagonDto? Wagon { get; set; }
        public SeatDto? Seat { get; set; }
        public TrainScheduleDto? TrainSchedule { get; set; }
    }
}
