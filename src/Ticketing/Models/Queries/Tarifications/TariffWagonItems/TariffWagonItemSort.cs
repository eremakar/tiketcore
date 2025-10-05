using Data.Repository;
using Ticketing.Data.TicketDb.Entities.Tarifications;

namespace Ticketing.Models.Queries.Tarifications.TariffWagonItems
{
    /// <summary>
    /// Элемент тарифа вагона
    /// </summary>
    public partial class TariffWagonItemSort : SortBase<TariffWagonItem>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? IndexCoefficient { get; set; }
        public SortOperand? WagonId { get; set; }
        public SortOperand? TariffId { get; set; }
    }
}
