using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;

namespace Ticketing.Tarifications.Models.Queries.Tickets
{
    /// <summary>
    /// Билет
    /// </summary>
    public partial class TicketSort : SortBase<Ticket>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? Number { get; set; }
        public SortOperand? Date { get; set; }
        public SortOperand? IsSeat { get; set; }
        public SortOperand? Price { get; set; }
        public SortOperand? State { get; set; }
        public SortOperand? Type { get; set; }
        public SortOperand? Total { get; set; }
        public SortOperand? FromId { get; set; }
        public SortOperand? ToId { get; set; }
        public SortOperand? TrainId { get; set; }
        public SortOperand? WagonId { get; set; }
        public SortOperand? SeatId { get; set; }
        public SortOperand? TrainScheduleId { get; set; }
    }
}
