using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities.Tarifications;

namespace Ticketing.Tarifications.Models.Queries.Tarifications.TrainCategories
{
    /// <summary>
    /// Категория поезда
    /// </summary>
    public partial class TrainCategorySort : SortBase<TrainCategory>
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
