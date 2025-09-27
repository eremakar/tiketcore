using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.TrainSchedules
{
    public partial class TrainScheduleQuery : QueryBase<TrainSchedule, TrainScheduleFilter, TrainScheduleSort>
    {
    }
}
