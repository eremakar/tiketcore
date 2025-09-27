using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.TrainSchedules
{
    /// <summary>
    /// Расписание поезда по дням
    /// </summary>
    public partial class TrainScheduleFilter : FilterBase<TrainSchedule>
    {
        public FilterOperand<long>? Id { get; set; }
        public FilterOperand<DateTime>? Date { get; set; }
        public FilterOperand<bool>? Active { get; set; }
        public FilterOperand<long?>? TrainId { get; set; }
    }
}
