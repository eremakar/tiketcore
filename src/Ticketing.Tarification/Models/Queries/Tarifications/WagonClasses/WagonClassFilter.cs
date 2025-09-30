using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities.Tarifications;

namespace Ticketing.Tarifications.Models.Queries.Tarifications.WagonClasses
{
    /// <summary>
    /// Класс вагона
    /// </summary>
    public partial class WagonClassFilter : FilterBase<WagonClass>
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
