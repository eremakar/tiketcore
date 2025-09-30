using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;

namespace Ticketing.Tarifications.Models.Queries.TicketStates
{
    /// <summary>
    /// Статус билета
    /// </summary>
    public partial class TicketStateFilter : FilterBase<TicketState>
    {
        public FilterOperand<long>? Id { get; set; }
        public FilterOperand<string>? Name { get; set; }
        public FilterOperand<string>? Code { get; set; }
    }
}
