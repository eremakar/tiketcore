using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;

namespace Ticketing.Tarifications.Models.Queries.SeatCountSegments
{
    /// <summary>
    /// Сегмент по количеству мест
    /// </summary>
    public partial class SeatCountSegmentFilter : FilterBase<SeatCountSegment>
    {
        public FilterOperand<long>? Id { get; set; }
        public FilterOperand<int>? SeatCount { get; set; }
        public FilterOperand<int>? FreeCount { get; set; }
        public FilterOperand<double>? Price { get; set; }
        public FilterOperand<object>? Tickets { get; set; }
        public FilterOperand<long?>? FromId { get; set; }
        public FilterOperand<long?>? ToId { get; set; }
        public FilterOperand<long?>? TrainId { get; set; }
        public FilterOperand<long?>? WagonId { get; set; }
        public FilterOperand<long?>? TrainScheduleId { get; set; }
    }
}
