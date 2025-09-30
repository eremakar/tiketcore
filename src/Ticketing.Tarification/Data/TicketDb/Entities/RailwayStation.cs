using Data.Repository;

namespace Ticketing.Tarifications.Data.TicketDb.Entities
{
    /// <summary>
    /// Станция дороги
    /// </summary>
    public partial class RailwayStation : IEntityKey<long>
    {
        public long Id { get; set; }
        public int Order { get; set; }
        public double Distance { get; set; }
        public long? StationId { get; set; }
        public long? RailwayId { get; set; }

        public Station? Station { get; set; }
        public Railway? Railway { get; set; }
    }
}
