using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;

namespace Ticketing.Tarifications.Models.Queries.Seats
{
    /// <summary>
    /// Место в вагоне
    /// </summary>
    public partial class SeatSort : SortBase<Seat>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? Number { get; set; }
        public SortOperand? Class { get; set; }
        public SortOperand? WagonId { get; set; }
        /// <summary>
        /// Тип места: верхний/боковой/нижний
        /// </summary>
        public SortOperand? TypeId { get; set; }
    }
}
