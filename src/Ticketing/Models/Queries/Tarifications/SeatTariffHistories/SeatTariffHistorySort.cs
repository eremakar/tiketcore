using Data.Repository;
using Ticketing.Data.TicketDb.Entities.Tarifications;

namespace Ticketing.Models.Queries.Tarifications.SeatTariffHistories
{
    /// <summary>
    /// История тарифа места в вагоне
    /// </summary>
    public partial class SeatTariffHistorySort : SortBase<SeatTariffHistory>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? Name { get; set; }
        public SortOperand? Price { get; set; }
        public SortOperand? DateTime { get; set; }
        public SortOperand? BaseFareId { get; set; }
        public SortOperand? TrainId { get; set; }
        public SortOperand? TrainCategoryId { get; set; }
        public SortOperand? WagonClassId { get; set; }
        public SortOperand? SeasonId { get; set; }
        public SortOperand? SeatTypeId { get; set; }
        /// <summary>
        /// соединение станций
        /// </summary>
        public SortOperand? ConnectionId { get; set; }
    }
}
