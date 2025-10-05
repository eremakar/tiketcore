using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities.Tarifications;

namespace Ticketing.Tarifications.Models.Queries.Tarifications.SeatTariffItems
{
    /// <summary>
    /// Элемент тарифа места
    /// </summary>
    public partial class SeatTariffItemFilter : FilterBase<SeatTariffItem>
    {
        public FilterOperand<long>? Id { get; set; }
        public FilterOperand<object>? CalculationParameters { get; set; }
        public FilterOperand<double>? Distance { get; set; }
        public FilterOperand<double>? Price { get; set; }
        public FilterOperand<long?>? WagonClassId { get; set; }
        public FilterOperand<long?>? SeasonId { get; set; }
        public FilterOperand<long?>? WagonId { get; set; }
        public FilterOperand<long?>? SeatTypeId { get; set; }
        public FilterOperand<long?>? FromId { get; set; }
        public FilterOperand<long?>? ToId { get; set; }
        public FilterOperand<long?>? SeatTariffId { get; set; }
    }
}
