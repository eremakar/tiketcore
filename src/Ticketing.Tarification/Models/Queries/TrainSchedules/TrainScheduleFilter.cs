using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;

namespace Ticketing.Tarifications.Models.Queries.TrainSchedules
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
        public FilterOperand<long?>? SeatTariffId { get; set; }
    }
}
