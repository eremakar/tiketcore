using Data.Repository;

namespace Ticketing.Data.TicketDb.Entities
{
    /// <summary>
    /// Билет
    /// </summary>
    public partial class Ticket : IEntityKey<long>
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

        public RouteStation? From { get; set; }
        public RouteStation? To { get; set; }
        public Train? Train { get; set; }
        public TrainWagon? Wagon { get; set; }
        public Seat? Seat { get; set; }
        public TrainSchedule? TrainSchedule { get; set; }
    }
}
