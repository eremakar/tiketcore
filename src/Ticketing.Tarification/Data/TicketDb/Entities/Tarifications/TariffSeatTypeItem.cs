using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;

namespace Ticketing.Tarifications.Data.TicketDb.Entities.Tarifications
{
    /// <summary>
    /// Элемент тарифа типа места
    /// </summary>
    public partial class TariffSeatTypeItem : IEntityKey<long>
    {
        public long Id { get; set; }
        public double IndexCoefficient { get; set; }
        public long? SeatTypeId { get; set; }
        public long? TariffWagonId { get; set; }

        public SeatType? SeatType { get; set; }
        public TariffWagonItem? TariffWagon { get; set; }
    }
}
