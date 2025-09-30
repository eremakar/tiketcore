using System.Text.Json;
using Data.Repository;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketing.Tarifications.Data.TicketDb.Entities
{
    /// <summary>
    /// Сегмент по количеству мест
    /// </summary>
    public partial class SeatCountSegment : IEntityKey<long>
    {
        public long Id { get; set; }
        public int SeatCount { get; set; }
        public int FreeCount { get; set; }
        public double Price { get; set; }
        [Column(TypeName = "jsonb")]
        public string? Tickets { get; set; }
        public long? FromId { get; set; }
        public long? ToId { get; set; }
        public long? TrainId { get; set; }
        public long? WagonId { get; set; }
        public long? TrainScheduleId { get; set; }

        public RouteStation? From { get; set; }
        public RouteStation? To { get; set; }
        public Train? Train { get; set; }
        public TrainWagon? Wagon { get; set; }
        public TrainSchedule? TrainSchedule { get; set; }

        [InverseProperty("Segments")]
        public List<SeatCountReservation>? SeatCountReservation { get; set; }
    }
}
