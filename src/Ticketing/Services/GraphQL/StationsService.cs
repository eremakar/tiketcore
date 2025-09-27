using HotChocolate;
using HotChocolate.Authorization;
using Ticketing.Models.Dtos;

namespace Ticketing.Services.GraphQL
{
    public class StationsService : RestService2<Station, long, StationDto, StationQuery, StationMap>
    {
        private readonly TicketDbContext db;

        public StationsService(ILogger<RestServiceBase<Station, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            StationMap map)
            : base(logger,
                restDapperDb,
                restDb,
                "Stations",
                map)
        {
            this.db = restDb;
        }

        public override async Task<PagedList<StationDto>> SearchAsync(StationQuery query)
        {
            return await base.SearchAsync(query);
        }
    }
}
