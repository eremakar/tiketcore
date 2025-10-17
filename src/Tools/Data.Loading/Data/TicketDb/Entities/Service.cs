using Data.Repository;

namespace Ticketing.Data.TicketDb.Entities
{
    /// <summary>
    /// Сервис/услуга
    /// </summary>
    public partial class Service : IEntityKey<long>
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
    }
}
