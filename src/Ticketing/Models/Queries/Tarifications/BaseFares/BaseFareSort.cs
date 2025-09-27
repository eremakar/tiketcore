using Data.Repository;
using Ticketing.Data.TicketDb.Entities.Tarifications;

namespace Ticketing.Models.Queries.Tarifications.BaseFares
{
    /// <summary>
    /// Базовая ставка
    /// </summary>
    public partial class BaseFareSort : SortBase<BaseFare>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? Name { get; set; }
        public SortOperand? Price { get; set; }
    }
}
