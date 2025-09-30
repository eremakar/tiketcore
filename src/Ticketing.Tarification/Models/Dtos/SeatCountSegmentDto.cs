
namespace Ticketing.Tarifications.Models.Dtos
{
    /// <summary>
    /// Сегмент по количеству мест
    /// </summary>
    public partial class SeatCountSegmentDto
    {
        public long Id { get; set; }
        public int SeatCount { get; set; }
        public int FreeCount { get; set; }
        public double Price { get; set; }
        public object? Tickets { get; set; }
        public long? FromId { get; set; }
        public long? ToId { get; set; }
        public long? TrainId { get; set; }
        public long? WagonId { get; set; }
        public long? TrainScheduleId { get; set; }

        public RouteStationDto? From { get; set; }
        public RouteStationDto? To { get; set; }
        public TrainDto? Train { get; set; }
        public TrainWagonDto? Wagon { get; set; }
        public TrainScheduleDto? TrainSchedule { get; set; }

        public List<SeatCountReservationDto>? SeatCountReservation { get; set; }
    }
}
