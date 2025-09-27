using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.SeatTypes
{
    /// <summary>
    /// Тип места
    /// </summary>
    public partial class SeatTypeSort : SortBase<SeatType>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? Name { get; set; }
        public SortOperand? Code { get; set; }
        /// <summary>
        /// Тарифный коэффициент
        /// </summary>
        public SortOperand? TarifCoefficient { get; set; }
    }
}
