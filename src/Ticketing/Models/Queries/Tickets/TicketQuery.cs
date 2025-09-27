using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.Tickets
{
    public partial class TicketQuery : QueryBase<Ticket, TicketFilter, TicketSort>
    {
    }
}
