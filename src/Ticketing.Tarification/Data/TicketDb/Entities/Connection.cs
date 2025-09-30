using Data.Repository;

namespace Ticketing.Tarifications.Data.TicketDb.Entities
{
    /// <summary>
    /// Соединение 2х станций
    /// </summary>
    public partial class Connection : IEntityKey<long>
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public double DistanceKm { get; set; }
        public long? FromId { get; set; }
        public long? ToId { get; set; }

        public Station? From { get; set; }
        public Station? To { get; set; }
    }
}
