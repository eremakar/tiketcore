using HotChocolate;
using HotChocolate.Authorization;
using Ticketing.Models.Dtos;

namespace Ticketing.Services.GraphQL
{
    public class TrainsService : RestService2<Train, long, TrainDto, TrainQuery, TrainMap>
    {
        private readonly TicketDbContext db;

        public TrainsService(ILogger<RestServiceBase<Train, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            TrainMap map)
            : base(logger,
                restDapperDb,
                restDb,
                "Trains",
                map)
        {
            this.db = restDb;
        }

        public override async Task<PagedList<TrainDto>> SearchAsync(TrainQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.From).
                Include(_ => _.To).
                Include(_ => _.Route));
        }
    }
}
