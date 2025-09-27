using HotChocolate;
using HotChocolate.Authorization;
using Ticketing.Models.Dtos;

namespace Ticketing.Services.GraphQL
{
    public class SegmentsService : RestService2<Segment, long, SegmentDto, SegmentQuery, SegmentMap>
    {
        private readonly TicketDbContext db;

        public SegmentsService(ILogger<RestServiceBase<Segment, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            SegmentMap map)
            : base(logger,
                restDapperDb,
                restDb,
                "Segments",
                map)
        {
            this.db = restDb;
        }

        public override async Task<PagedList<SegmentDto>> SearchAsync(SegmentQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.Seat).
                Include(_ => _.From).
                Include(_ => _.To).
                Include(_ => _.Train).
                Include(_ => _.Wagon).
                Include(_ => _.TrainSchedule).
                Include(_ => _.Ticket).
                Include(_ => _.Reservation));
        }
    }
}
