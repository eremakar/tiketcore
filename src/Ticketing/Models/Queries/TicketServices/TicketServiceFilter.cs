using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.TicketServices
{
    /// <summary>
    /// Услуги билета
    /// </summary>
    public partial class TicketServiceFilter : FilterBase<TicketService>
    {
        public FilterOperand<long>? Id { get; set; }
        public FilterOperand<int>? State { get; set; }
        public FilterOperand<long?>? TicketId { get; set; }
        public FilterOperand<long?>? ServiceId { get; set; }
    }
}
