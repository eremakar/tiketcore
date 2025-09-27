using HotChocolate;
using HotChocolate.Authorization;
using Ticketing.Models.Dtos;

namespace Ticketing.Services.GraphQL
{
    public class TicketsService : RestService2<Ticket, long, TicketDto, TicketQuery, TicketMap>
    {
        private readonly TicketDbContext db;

        public TicketsService(ILogger<RestServiceBase<Ticket, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            TicketMap map)
            : base(logger,
                restDapperDb,
                restDb,
                "Tickets",
                map)
        {
            this.db = restDb;
        }

        public override async Task<PagedList<TicketDto>> SearchAsync(TicketQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.From).
                Include(_ => _.To).
                Include(_ => _.Train).
                Include(_ => _.Wagon).
                Include(_ => _.Seat).
                Include(_ => _.TrainSchedule));
        }
    }
}
