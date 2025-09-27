
namespace Ticketing.Models.Dtos
{
    /// <summary>
    /// Сегмент по месту (от-до)
    /// </summary>
    public partial class SeatSegmentDto
    {
        public long Id { get; set; }
        public double Price { get; set; }
        public long? SeatId { get; set; }
        public long? FromId { get; set; }
        public long? ToId { get; set; }
        public long? TrainId { get; set; }
        public long? WagonId { get; set; }
        public long? TrainScheduleId { get; set; }
        public long? TicketId { get; set; }
        public long? SeatReservationId { get; set; }

        public SeatDto? Seat { get; set; }
        public RouteStationDto? From { get; set; }
        public RouteStationDto? To { get; set; }
        public TrainDto? Train { get; set; }
        public TrainWagonDto? Wagon { get; set; }
        public TrainScheduleDto? TrainSchedule { get; set; }
        public TicketDto? Ticket { get; set; }
        public SeatReservationDto? SeatReservation { get; set; }
    }
}
