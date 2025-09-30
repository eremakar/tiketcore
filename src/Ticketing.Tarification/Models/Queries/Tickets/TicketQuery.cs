using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;

namespace Ticketing.Tarifications.Models.Queries.Tickets
{
    public partial class TicketQuery : QueryBase<Ticket, TicketFilter, TicketSort>
    {
    }
}
