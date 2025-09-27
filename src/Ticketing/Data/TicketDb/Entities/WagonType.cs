using Data.Repository;

namespace Ticketing.Data.TicketDb.Entities
{
    /// <summary>
    /// Тип вагона
    /// </summary>
    public partial class WagonType : IEntityKey<long>
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? ShortName { get; set; }
        public string? Code { get; set; }
    }
}
