using Data.Repository;

namespace Ticketing.Data.TicketDb.Entities.Dictionaries
{
    /// <summary>
    /// Периодичность
    /// </summary>
    public partial class Periodicity : IEntityKey<long>
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
    }
}
