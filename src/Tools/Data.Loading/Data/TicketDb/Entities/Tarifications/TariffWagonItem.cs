using Data.Repository;
using Ticketing.Data.TicketDb.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketing.Data.TicketDb.Entities.Tarifications
{
    /// <summary>
    /// Элемент тарифа вагона
    /// </summary>
    public partial class TariffWagonItem : IEntityKey<long>
    {
        public long Id { get; set; }
        public double IndexCoefficient { get; set; }
        public long? WagonId { get; set; }
        public long? TariffId { get; set; }

        public WagonModel? Wagon { get; set; }
        public Tariff? Tariff { get; set; }

        [InverseProperty("TariffWagon")]
        public List<TariffSeatTypeItem>? SeatTypes { get; set; }
    }
}
