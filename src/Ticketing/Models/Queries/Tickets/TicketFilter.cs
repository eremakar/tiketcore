using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.Tickets
{
    /// <summary>
    /// Билет
    /// </summary>
    public partial class TicketFilter : FilterBase<Ticket>
    {
        public FilterOperand<long>? Id { get; set; }
        public FilterOperand<string>? Number { get; set; }
        public FilterOperand<DateTime>? Date { get; set; }
        public FilterOperand<bool>? IsSeat { get; set; }
        public FilterOperand<double>? Price { get; set; }
        public FilterOperand<int>? State { get; set; }
        public FilterOperand<int>? Type { get; set; }
        public FilterOperand<string>? Total { get; set; }
        public FilterOperand<long?>? FromId { get; set; }
        public FilterOperand<long?>? ToId { get; set; }
        public FilterOperand<long?>? TrainId { get; set; }
        public FilterOperand<long?>? WagonId { get; set; }
        public FilterOperand<long?>? SeatId { get; set; }
        public FilterOperand<long?>? TrainScheduleId { get; set; }
    }
}
