using HotChocolate;
using HotChocolate.Authorization;
using Ticketing.Models.Dtos;

namespace Ticketing.Services.GraphQL
{
    public class SeatsService : RestService2<Seat, long, SeatDto, SeatQuery, SeatMap>
    {
        private readonly TicketDbContext db;

        public SeatsService(ILogger<RestServiceBase<Seat, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            SeatMap map)
            : base(logger,
                restDapperDb,
                restDb,
                "Seats",
                map)
        {
            this.db = restDb;
        }

        public override async Task<PagedList<SeatDto>> SearchAsync(SeatQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.Wagon));
        }
    }
}
