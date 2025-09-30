using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;

namespace Ticketing.Tarifications.Models.Queries.SeatReservations
{
    /// <summary>
    /// Бронирование места
    /// </summary>
    public partial class SeatReservationSort : SortBase<SeatReservation>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? Number { get; set; }
        public SortOperand? Date { get; set; }
        public SortOperand? Price { get; set; }
        public SortOperand? Total { get; set; }
        public SortOperand? FromId { get; set; }
        public SortOperand? ToId { get; set; }
        public SortOperand? TrainId { get; set; }
        public SortOperand? WagonId { get; set; }
        public SortOperand? SeatId { get; set; }
        public SortOperand? TrainScheduleId { get; set; }
    }
}
