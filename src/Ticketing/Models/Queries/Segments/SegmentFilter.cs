using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.Segments
{
    /// <summary>
    /// Сегмент поездки (от-до)
    /// </summary>
    public partial class SegmentFilter : FilterBase<Segment>
    {
        public FilterOperand<long>? Id { get; set; }
        public FilterOperand<long?>? SeatId { get; set; }
        public FilterOperand<long?>? FromId { get; set; }
        public FilterOperand<long?>? ToId { get; set; }
        public FilterOperand<long?>? TrainId { get; set; }
        public FilterOperand<long?>? WagonId { get; set; }
        public FilterOperand<long?>? TrainScheduleId { get; set; }
        public FilterOperand<long?>? TicketId { get; set; }
        public FilterOperand<long?>? ReservationId { get; set; }
    }
}
