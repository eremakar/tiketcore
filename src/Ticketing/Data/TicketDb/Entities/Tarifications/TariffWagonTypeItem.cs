using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Data.TicketDb.Entities.Tarifications
{
    /// <summary>
    /// Элемент тарифа типа вагона
    /// </summary>
    public partial class TariffWagonTypeItem : IEntityKey<long>
    {
        public long Id { get; set; }
        public double IndexCoefficient { get; set; }
        public long? WagonTypeId { get; set; }
        public long? TariffId { get; set; }

        public WagonType? WagonType { get; set; }
        public Tariff? Tariff { get; set; }
    }
}
