using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities.Tarifications;

namespace Ticketing.Tarifications.Models.Queries.Tarifications.TariffTrainCategoryItems
{
    /// <summary>
    /// Элемент тарифа категории поезда
    /// </summary>
    public partial class TariffTrainCategoryItemFilter : FilterBase<TariffTrainCategoryItem>
    {
        public FilterOperand<long>? Id { get; set; }
        public FilterOperand<double>? IndexCoefficient { get; set; }
        public FilterOperand<long?>? TrainCategoryId { get; set; }
        public FilterOperand<long?>? TariffId { get; set; }
    }
}
