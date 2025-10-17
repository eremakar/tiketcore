using Data.Repository;

namespace Ticketing.Data.TicketDb.Entities
{
    /// <summary>
    /// Особенности вагона
    /// </summary>
    public partial class WagonFeature : IEntityKey<int>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
    }
}
