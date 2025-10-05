using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.WagonModelFeatures
{
    /// <summary>
    /// Особенности модели вагона
    /// </summary>
    public partial class WagonModelFeatureFilter : FilterBase<WagonModelFeature>
    {
        public FilterOperand<long>? Id { get; set; }
        public FilterOperand<long?>? WagonId { get; set; }
        public FilterOperand<int?>? FeatureId { get; set; }
    }
}
