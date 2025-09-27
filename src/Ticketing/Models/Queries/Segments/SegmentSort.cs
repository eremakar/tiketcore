using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.Segments
{
    /// <summary>
    /// Сегмент поездки (от-до)
    /// </summary>
    public partial class SegmentSort : SortBase<Segment>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? SeatId { get; set; }
        public SortOperand? FromId { get; set; }
        public SortOperand? ToId { get; set; }
        public SortOperand? TrainId { get; set; }
        public SortOperand? WagonId { get; set; }
        public SortOperand? TrainScheduleId { get; set; }
        public SortOperand? TicketId { get; set; }
        public SortOperand? ReservationId { get; set; }
    }
}
