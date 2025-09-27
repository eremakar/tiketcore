using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.SeatCountReservations
{
    /// <summary>
    /// Бронирование количества мест
    /// </summary>
    public partial class SeatCountReservationFilter : FilterBase<SeatCountReservation>
    {
        public FilterOperand<long>? Id { get; set; }
        public FilterOperand<string>? Number { get; set; }
        public FilterOperand<DateTime>? DateTime { get; set; }
        public FilterOperand<double>? Price { get; set; }
        public FilterOperand<string>? Total { get; set; }
        public FilterOperand<int>? SeatCount { get; set; }
        public FilterOperand<long?>? FromId { get; set; }
        public FilterOperand<long?>? ToId { get; set; }
        public FilterOperand<long?>? TrainId { get; set; }
        public FilterOperand<long?>? WagonId { get; set; }
        public FilterOperand<long?>? TrainScheduleId { get; set; }
    }
}
