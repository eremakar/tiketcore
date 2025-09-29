using Data.Repository;
using Ticketing.Data.TicketDb.Entities.Tarifications;

namespace Ticketing.Models.Queries.Tarifications.SeatTariffItems
{
    /// <summary>
    /// Элемент тарифа места
    /// </summary>
    public partial class SeatTariffItemSort : SortBase<SeatTariffItem>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? Distance { get; set; }
        public SortOperand? Price { get; set; }
        public SortOperand? WagonClassId { get; set; }
        public SortOperand? SeasonId { get; set; }
        public SortOperand? SeatTypeId { get; set; }
        public SortOperand? FromId { get; set; }
        public SortOperand? ToId { get; set; }
        public SortOperand? SeatTariffId { get; set; }
    }
}
