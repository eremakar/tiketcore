using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities.Tarifications;

namespace Ticketing.Tarifications.Models.Queries.Tarifications.TariffSeatTypeItems
{
    /// <summary>
    /// Элемент тарифа типа места
    /// </summary>
    public partial class TariffSeatTypeItemFilter : FilterBase<TariffSeatTypeItem>
    {
        public FilterOperand<long>? Id { get; set; }
        public FilterOperand<double>? IndexCoefficient { get; set; }
        public FilterOperand<long?>? SeatTypeId { get; set; }
        public FilterOperand<long?>? TariffWagonId { get; set; }
    }
}
