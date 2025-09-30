using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;

namespace Ticketing.Data.TicketDb.Entities
{
    /// <summary>
    /// Сегмент поездки (от-до)
    /// </summary>
    public partial class Segment : IEntityKey<long>
    {
        public long Id { get; set; }
        public long? SeatId { get; set; }
        public long? FromId { get; set; }
        public long? ToId { get; set; }
        public long? TrainId { get; set; }
        public long? WagonId { get; set; }
        public long? TrainScheduleId { get; set; }
        public long? TicketId { get; set; }
        public long? ReservationId { get; set; }

        public Seat? Seat { get; set; }
        public RouteStation? From { get; set; }
        public RouteStation? To { get; set; }
        public Train? Train { get; set; }
        public TrainWagon? Wagon { get; set; }
        public TrainSchedule? TrainSchedule { get; set; }
        public Ticket? Ticket { get; set; }
        public SeatReservation? Reservation { get; set; }
    }
}
