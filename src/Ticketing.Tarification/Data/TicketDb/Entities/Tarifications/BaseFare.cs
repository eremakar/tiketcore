using Data.Repository;

namespace Ticketing.Tarifications.Data.TicketDb.Entities.Tarifications
{
    /// <summary>
    /// Базовая ставка
    /// </summary>
    public partial class BaseFare : IEntityKey<long>
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public double Price { get; set; }
    }
}
