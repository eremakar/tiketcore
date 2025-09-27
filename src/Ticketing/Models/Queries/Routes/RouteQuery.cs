using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.Routes
{
    public partial class RouteQuery : QueryBase<Data.TicketDb.Entities.Route, RouteFilter, RouteSort>
    {
    }
}
