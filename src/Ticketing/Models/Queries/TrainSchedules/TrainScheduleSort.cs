using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.TrainSchedules
{
    /// <summary>
    /// Расписание поезда по дням
    /// </summary>
    public partial class TrainScheduleSort : SortBase<TrainSchedule>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? Date { get; set; }
        public SortOperand? Active { get; set; }
        public SortOperand? TrainId { get; set; }
    }
}
