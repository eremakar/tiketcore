using Data.Repository;
using Ticketing.Data.TicketDb.Entities.Tarifications;

namespace Ticketing.Models.Queries.Tarifications.TariffWagonTypeItems
{
    /// <summary>
    /// Элемент тарифа типа вагона
    /// </summary>
    public partial class TariffWagonTypeItemSort : SortBase<TariffWagonTypeItem>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? IndexCoefficient { get; set; }
        public SortOperand? WagonTypeId { get; set; }
        public SortOperand? TariffId { get; set; }
    }
}
