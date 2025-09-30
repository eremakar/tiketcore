using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;

namespace Ticketing.Tarifications.Models.Queries.SeatCountSegments
{
    /// <summary>
    /// Сегмент по количеству мест
    /// </summary>
    public partial class SeatCountSegmentSort : SortBase<SeatCountSegment>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? SeatCount { get; set; }
        public SortOperand? FreeCount { get; set; }
        public SortOperand? Price { get; set; }
        public SortOperand? Tickets { get; set; }
        public SortOperand? FromId { get; set; }
        public SortOperand? ToId { get; set; }
        public SortOperand? TrainId { get; set; }
        public SortOperand? WagonId { get; set; }
        public SortOperand? TrainScheduleId { get; set; }
    }
}
