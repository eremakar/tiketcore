using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.TimeSchedules
{
    /// <summary>
    /// График времени на станции
    /// </summary>
    public partial class TimeScheduleSort : SortBase<TimeSchedule>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? Arrival { get; set; }
        public SortOperand? Stop { get; set; }
        public SortOperand? Departure { get; set; }
        public SortOperand? TrainId { get; set; }
        public SortOperand? RouteStationId { get; set; }
    }
}
