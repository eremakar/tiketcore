using Data.Repository;

namespace Ticketing.Tarifications.Data.TicketDb.Entities
{
    /// <summary>
    /// Статус билета
    /// </summary>
    public partial class TicketState : IEntityKey<long>
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
    }
}
