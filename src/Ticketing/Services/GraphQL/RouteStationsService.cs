using HotChocolate;
using HotChocolate.Authorization;
using Ticketing.Models.Dtos;

namespace Ticketing.Services.GraphQL
{
    public class RouteStationsService : RestService2<RouteStation, long, RouteStationDto, RouteStationQuery, RouteStationMap>
    {
        private readonly TicketDbContext db;

        public RouteStationsService(ILogger<RestServiceBase<RouteStation, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            RouteStationMap map)
            : base(logger,
                restDapperDb,
                restDb,
                "RouteStations",
                map)
        {
            this.db = restDb;
        }

        public override async Task<PagedList<RouteStationDto>> SearchAsync(RouteStationQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.Station).
                Include(_ => _.Route));
        }
    }
}
