using Data.Repository;
using Ticketing.Data.TicketDb.Entities.Tarifications;

namespace Ticketing.Models.Queries.Tarifications.TrainCategories
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
