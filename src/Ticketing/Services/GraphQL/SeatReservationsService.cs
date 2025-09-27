using HotChocolate;
using HotChocolate.Authorization;
using Ticketing.Models.Dtos;

namespace Ticketing.Services.GraphQL
{
    public class SeatReservationsService : RestService2<SeatReservation, long, SeatReservationDto, SeatReservationQuery, SeatReservationMap>
    {
        private readonly TicketDbContext db;

        public SeatReservationsService(ILogger<RestServiceBase<SeatReservation, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            SeatReservationMap map)
            : base(logger,
                restDapperDb,
                restDb,
                "SeatReservations",
                map)
        {
            this.db = restDb;
        }

        public override async Task<PagedList<SeatReservationDto>> SearchAsync(SeatReservationQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.From).
                Include(_ => _.To).
                Include(_ => _.Train).
                Include(_ => _.Wagon).
                Include(_ => _.Seat).
                Include(_ => _.TrainSchedule).
                Include(_ => _.Segments));
        }
    }
}
