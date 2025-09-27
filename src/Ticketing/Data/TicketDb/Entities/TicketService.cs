using Data.Repository;

namespace Ticketing.Data.TicketDb.Entities
{
    /// <summary>
    /// Услуги билета
    /// </summary>
    public partial class TicketService : IEntityKey<long>
    {
        public long Id { get; set; }
        public int State { get; set; }
        public long? TicketId { get; set; }
        public long? ServiceId { get; set; }

        public Ticket? Ticket { get; set; }
        public Service? Service { get; set; }
    }
}
