using Data.Repository;
using Ticketing.Data.TicketDb.Entities.Tarifications;

namespace Ticketing.Models.Queries.Tarifications.TariffSeatTypeItems
{
    /// <summary>
    /// Элемент тарифа типа места
    /// </summary>
    public partial class TariffSeatTypeItemSort : SortBase<TariffSeatTypeItem>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? IndexCoefficient { get; set; }
        public SortOperand? SeatTypeId { get; set; }
        public SortOperand? TariffWagonId { get; set; }
    }
}
