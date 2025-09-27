using HotChocolate;
using HotChocolate.Authorization;
using Ticketing.Models.Dtos;

namespace Ticketing.Services.GraphQL
{
    public class TimeSchedulesService : RestService2<TimeSchedule, long, TimeScheduleDto, TimeScheduleQuery, TimeScheduleMap>
    {
        private readonly TicketDbContext db;

        public TimeSchedulesService(ILogger<RestServiceBase<TimeSchedule, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            TimeScheduleMap map)
            : base(logger,
                restDapperDb,
                restDb,
                "TimeSchedules",
                map)
        {
            this.db = restDb;
        }

        public override async Task<PagedList<TimeScheduleDto>> SearchAsync(TimeScheduleQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.Train).
                Include(_ => _.RouteStation));
        }
    }
}
