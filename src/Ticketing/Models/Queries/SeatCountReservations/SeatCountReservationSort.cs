using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.SeatCountReservations
{
    /// <summary>
    /// Бронирование количества мест
    /// </summary>
    public partial class SeatCountReservationSort : SortBase<SeatCountReservation>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? Number { get; set; }
        public SortOperand? DateTime { get; set; }
        public SortOperand? Price { get; set; }
        public SortOperand? Total { get; set; }
        public SortOperand? SeatCount { get; set; }
        public SortOperand? FromId { get; set; }
        public SortOperand? ToId { get; set; }
        public SortOperand? TrainId { get; set; }
        public SortOperand? WagonId { get; set; }
        public SortOperand? TrainScheduleId { get; set; }
    }
}
