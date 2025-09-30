using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities.Tarifications;

namespace Ticketing.Tarifications.Models.Queries.Tarifications.BaseFares
{
    /// <summary>
    /// Базовая ставка
    /// </summary>
    public partial class BaseFareFilter : FilterBase<BaseFare>
    {
        public FilterOperand<long>? Id { get; set; }
        public FilterOperand<string>? Name { get; set; }
        public FilterOperand<double>? Price { get; set; }
    }
}
