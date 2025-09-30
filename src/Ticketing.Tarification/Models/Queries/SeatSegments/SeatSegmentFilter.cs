using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;

namespace Ticketing.Tarifications.Models.Queries.SeatSegments
{
    /// <summary>
    /// Сегмент по месту (от-до)
    /// </summary>
    public partial class SeatSegmentFilter : FilterBase<SeatSegment>
    {
        public FilterOperand<long>? Id { get; set; }
        public FilterOperand<double>? Price { get; set; }
        public FilterOperand<DateTime>? Departure { get; set; }
        public FilterOperand<long?>? SeatId { get; set; }
        public FilterOperand<long?>? FromId { get; set; }
        public FilterOperand<long?>? ToId { get; set; }
        public FilterOperand<long?>? TrainId { get; set; }
        public FilterOperand<long?>? WagonId { get; set; }
        public FilterOperand<long?>? TrainScheduleId { get; set; }
        public FilterOperand<long?>? TicketId { get; set; }
        public FilterOperand<long?>? SeatReservationId { get; set; }
    }
}
