using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities.Tarifications;

namespace Ticketing.Tarifications.Models.Queries.Tarifications.TariffWagonItems
{
    /// <summary>
    /// Элемент тарифа вагона
    /// </summary>
    public partial class TariffWagonItemFilter : FilterBase<TariffWagonItem>
    {
        public FilterOperand<long>? Id { get; set; }
        public FilterOperand<double>? IndexCoefficient { get; set; }
        public FilterOperand<long?>? WagonId { get; set; }
        public FilterOperand<long?>? TariffId { get; set; }
    }
}
