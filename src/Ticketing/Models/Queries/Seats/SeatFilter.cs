using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.Seats
{
    /// <summary>
    /// Место в вагоне
    /// </summary>
    public partial class SeatFilter : FilterBase<Seat>
    {
        public FilterOperand<long>? Id { get; set; }
        public FilterOperand<string>? Number { get; set; }
        public FilterOperand<int>? Class { get; set; }
        public FilterOperand<long?>? WagonId { get; set; }
        /// <summary>
        /// Тип места: верхний/боковой/нижний
        /// </summary>
        public FilterOperand<long?>? TypeId { get; set; }
    }
}
