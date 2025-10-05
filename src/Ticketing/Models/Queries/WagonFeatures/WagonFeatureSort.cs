using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.WagonFeatures
{
    /// <summary>
    /// Особенности вагона
    /// </summary>
    public partial class WagonFeatureSort : SortBase<WagonFeature>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? Name { get; set; }
        public SortOperand? Code { get; set; }
    }
}
