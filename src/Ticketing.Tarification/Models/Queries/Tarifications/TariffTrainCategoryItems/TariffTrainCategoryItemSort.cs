using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities.Tarifications;

namespace Ticketing.Tarifications.Models.Queries.Tarifications.TariffTrainCategoryItems
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
