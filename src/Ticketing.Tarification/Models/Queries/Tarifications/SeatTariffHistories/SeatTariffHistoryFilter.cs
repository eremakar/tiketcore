using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities.Tarifications;

namespace Ticketing.Tarifications.Models.Queries.Tarifications.SeatTariffHistories
{
    /// <summary>
    /// История тарифа места в вагоне
    /// </summary>
    public partial class SeatTariffHistoryFilter : FilterBase<SeatTariffHistory>
    {
        public FilterOperand<long>? Id { get; set; }
        public FilterOperand<string>? Name { get; set; }
        public FilterOperand<double>? Price { get; set; }
        public FilterOperand<DateTime>? DateTime { get; set; }
        public FilterOperand<long?>? BaseFareId { get; set; }
        public FilterOperand<long?>? TrainId { get; set; }
        public FilterOperand<long?>? TrainCategoryId { get; set; }
        public FilterOperand<long?>? WagonClassId { get; set; }
        public FilterOperand<long?>? SeasonId { get; set; }
        public FilterOperand<long?>? SeatTypeId { get; set; }
        /// <summary>
        /// соединение станций
        /// </summary>
        public FilterOperand<long?>? ConnectionId { get; set; }
    }
}
