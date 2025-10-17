using Data.Repository;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketing.Data.TicketDb.Entities
{
    /// <summary>
    /// Бронирование места
    /// </summary>
    public partial class SeatReservation : IEntityKey<long>
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

        public RouteStation? From { get; set; }
        public RouteStation? To { get; set; }
        public Train? Train { get; set; }
        public TrainWagon? Wagon { get; set; }
        public Seat? Seat { get; set; }
        public TrainSchedule? TrainSchedule { get; set; }

        [InverseProperty("SeatReservation")]
        public List<SeatSegment>? Segments { get; set; }
    }
}
