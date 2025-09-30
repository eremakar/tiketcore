
namespace Ticketing.Tarifications.Models.Dtos
{
    /// <summary>
    /// Бронирование места
    /// </summary>
    public partial class SeatReservationDto
    {
        public long Id { get; set; }
        public string? Number { get; set; }
        public DateTime Date { get; set; }
        public double Price { get; set; }
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

        public List<SeatSegmentDto>? Segments { get; set; }
    }
}
