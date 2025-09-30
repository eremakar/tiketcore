using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;

namespace Ticketing.Tarifications.Models.Queries.Routes
{
    public partial class RouteQuery : QueryBase<Data.TicketDb.Entities.Route, RouteFilter, RouteSort>
    {
    }
}
