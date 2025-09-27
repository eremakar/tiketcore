using HotChocolate;
using HotChocolate.Authorization;
using Ticketing.Models.Dtos;

namespace Ticketing.Services.GraphQL
{
    public class RoutesService : RestService2<Route, long, RouteDto, RouteQuery, RouteMap>
    {
        private readonly TicketDbContext db;

        public RoutesService(ILogger<RestServiceBase<Route, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            RouteMap map)
            : base(logger,
                restDapperDb,
                restDb,
                "Routes",
                map)
        {
            this.db = restDb;
        }

        public override async Task<PagedList<RouteDto>> SearchAsync(RouteQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.Train).
                Include(_ => _.Stations));
        }
    }
}
