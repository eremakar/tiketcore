using HotChocolate;
using HotChocolate.Authorization;
using Ticketing.Models.Dtos;

namespace Ticketing.Services.GraphQL
{
    public class TrainSchedulesService : RestService2<TrainSchedule, long, TrainScheduleDto, TrainScheduleQuery, TrainScheduleMap>
    {
        private readonly TicketDbContext db;

        public TrainSchedulesService(ILogger<RestServiceBase<TrainSchedule, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            TrainScheduleMap map)
            : base(logger,
                restDapperDb,
                restDb,
                "TrainSchedules",
                map)
        {
            this.db = restDb;
        }

        public override async Task<PagedList<TrainScheduleDto>> SearchAsync(TrainScheduleQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.Train));
        }
    }
}
