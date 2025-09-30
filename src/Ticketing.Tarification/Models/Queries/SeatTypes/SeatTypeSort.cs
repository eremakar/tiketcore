using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;

namespace Ticketing.Tarifications.Models.Queries.SeatTypes
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
