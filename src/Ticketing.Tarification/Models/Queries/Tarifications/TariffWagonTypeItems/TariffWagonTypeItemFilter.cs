using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities.Tarifications;

namespace Ticketing.Tarifications.Models.Queries.Tarifications.TariffWagonTypeItems
{
    /// <summary>
    /// Элемент тарифа типа вагона
    /// </summary>
    public partial class TariffWagonTypeItemFilter : FilterBase<TariffWagonTypeItem>
    {
        public FilterOperand<long>? Id { get; set; }
        public FilterOperand<double>? IndexCoefficient { get; set; }
        public FilterOperand<long?>? WagonTypeId { get; set; }
        public FilterOperand<long?>? TariffId { get; set; }
    }
}
