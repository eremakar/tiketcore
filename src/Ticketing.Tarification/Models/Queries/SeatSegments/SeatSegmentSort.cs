using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;

namespace Ticketing.Tarifications.Models.Queries.SeatSegments
{
    /// <summary>
    /// Сегмент по месту (от-до)
    /// </summary>
    public partial class SeatSegmentSort : SortBase<SeatSegment>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? Price { get; set; }
        public SortOperand? Departure { get; set; }
        public SortOperand? SeatId { get; set; }
        public SortOperand? FromId { get; set; }
        public SortOperand? ToId { get; set; }
        public SortOperand? TrainId { get; set; }
        public SortOperand? WagonId { get; set; }
        public SortOperand? TrainScheduleId { get; set; }
        public SortOperand? TicketId { get; set; }
        public SortOperand? SeatReservationId { get; set; }
    }
}
