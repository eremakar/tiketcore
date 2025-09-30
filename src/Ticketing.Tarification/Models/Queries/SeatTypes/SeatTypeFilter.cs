using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;

namespace Ticketing.Tarifications.Models.Queries.SeatTypes
{
    /// <summary>
    /// Тип места
    /// </summary>
    public partial class SeatTypeFilter : FilterBase<SeatType>
    {
        public FilterOperand<long>? Id { get; set; }
        public FilterOperand<string>? Name { get; set; }
        public FilterOperand<string>? Code { get; set; }
        /// <summary>
        /// Тарифный коэффициент
        /// </summary>
        public FilterOperand<double>? TarifCoefficient { get; set; }
    }
}
