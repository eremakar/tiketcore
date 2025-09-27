using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.TimeSchedules
{
    /// <summary>
    /// График времени на станции
    /// </summary>
    public partial class TimeScheduleFilter : FilterBase<TimeSchedule>
    {
        public FilterOperand<long>? Id { get; set; }
        public FilterOperand<DateTime?>? Arrival { get; set; }
        public FilterOperand<int>? Stop { get; set; }
        public FilterOperand<DateTime?>? Departure { get; set; }
        public FilterOperand<long?>? TrainId { get; set; }
        public FilterOperand<long?>? RouteStationId { get; set; }
    }
}
