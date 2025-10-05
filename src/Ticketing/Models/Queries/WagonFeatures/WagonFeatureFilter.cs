using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.WagonFeatures
{
    /// <summary>
    /// Особенности вагона
    /// </summary>
    public partial class WagonFeatureFilter : FilterBase<WagonFeature>
    {
        public FilterOperand<int>? Id { get; set; }
        public FilterOperand<string>? Name { get; set; }
        public FilterOperand<string>? Code { get; set; }
    }
}
