using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities.Tarifications;

namespace Ticketing.Tarifications.Models.Queries.Tarifications.TrainCategories
{
    /// <summary>
    /// Категория поезда
    /// </summary>
    public partial class TrainCategoryFilter : FilterBase<TrainCategory>
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
