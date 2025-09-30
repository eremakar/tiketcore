using Data.Repository;

namespace Ticketing.Tarifications.Data.TicketDb.Entities
{
    /// <summary>
    /// Вокзал
    /// </summary>
    public partial class Depot : IEntityKey<long>
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public long? StationId { get; set; }

        public Station? Station { get; set; }
    }
}
