using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.TicketStates
{
    public partial class TicketStateQuery : QueryBase<TicketState, TicketStateFilter, TicketStateSort>
    {
    }
}
