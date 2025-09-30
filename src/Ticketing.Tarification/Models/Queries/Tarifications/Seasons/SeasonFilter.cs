using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities.Tarifications;

namespace Ticketing.Tarifications.Models.Queries.Tarifications.Seasons
{
    /// <summary>
    /// Сезонность
    /// </summary>
    public partial class SeasonFilter : FilterBase<Season>
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
