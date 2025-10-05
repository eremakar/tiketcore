using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.WagonModelFeatures
{
    /// <summary>
    /// Особенности модели вагона
    /// </summary>
    public partial class WagonModelFeatureSort : SortBase<WagonModelFeature>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? WagonId { get; set; }
        public SortOperand? FeatureId { get; set; }
    }
}
