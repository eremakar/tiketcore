using Data.Repository;
using Ticketing.Data.TicketDb.Entities.Tarifications;

namespace Ticketing.Models.Queries.Tarifications.SeatTariffItems
{
    /// <summary>
    /// Элемент тарифа места
    /// </summary>
    public partial class SeatTariffItemFilter : FilterBase<SeatTariffItem>
    {
        public FilterOperand<long>? Id { get; set; }
        public FilterOperand<double>? Distance { get; set; }
        public FilterOperand<double>? Price { get; set; }
        public FilterOperand<long?>? WagonClassId { get; set; }
        public FilterOperand<long?>? SeasonId { get; set; }
        public FilterOperand<long?>? SeatTypeId { get; set; }
        public FilterOperand<long?>? FromId { get; set; }
        public FilterOperand<long?>? ToId { get; set; }
        public FilterOperand<long?>? SeatTariffId { get; set; }
    }
}
