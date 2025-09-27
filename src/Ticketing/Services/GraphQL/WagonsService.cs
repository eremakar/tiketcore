using HotChocolate;
using HotChocolate.Authorization;
using Ticketing.Models.Dtos;

namespace Ticketing.Services.GraphQL
{
    public class WagonsService : RestService2<Wagon, long, WagonDto, WagonQuery, WagonMap>
    {
        private readonly TicketDbContext db;

        public WagonsService(ILogger<RestServiceBase<Wagon, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            WagonMap map)
            : base(logger,
                restDapperDb,
                restDb,
                "Wagons",
                map)
        {
            this.db = restDb;
        }

        public override async Task<PagedList<WagonDto>> SearchAsync(WagonQuery query)
        {
            return await base.SearchAsync(query);
        }
    }
}
