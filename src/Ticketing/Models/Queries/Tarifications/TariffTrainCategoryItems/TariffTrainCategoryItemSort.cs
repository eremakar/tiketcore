using Data.Repository;
using Ticketing.Data.TicketDb.Entities.Tarifications;

namespace Ticketing.Models.Queries.Tarifications.TariffTrainCategoryItems
{
    /// <summary>
    /// Элемент тарифа категории поезда
    /// </summary>
    public partial class TariffTrainCategoryItemSort : SortBase<TariffTrainCategoryItem>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? IndexCoefficient { get; set; }
        public SortOperand? TrainCategoryId { get; set; }
        public SortOperand? TariffId { get; set; }
    }
}
