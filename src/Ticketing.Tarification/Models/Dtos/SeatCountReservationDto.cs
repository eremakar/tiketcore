
namespace Ticketing.Tarifications.Models.Dtos
{
    /// <summary>
    /// Бронирование количества мест
    /// </summary>
    public partial class SeatCountReservationDto
    {
        public long Id { get; set; }
        public string? Number { get; set; }
        public DateTime DateTime { get; set; }
        public double Price { get; set; }
        public string? Total { get; set; }
        public int SeatCount { get; set; }
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

        public List<SeatCountSegmentDto>? Segments { get; set; }
    }
}
