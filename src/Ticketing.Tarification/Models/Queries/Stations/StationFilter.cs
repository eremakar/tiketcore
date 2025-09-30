using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;

namespace Ticketing.Tarifications.Models.Queries.Stations
{
    /// <summary>
    /// Станция
    /// </summary>
    public partial class StationFilter : FilterBase<Station>
    {
        public FilterOperand<long>? Id { get; set; }
        public FilterOperand<string>? Name { get; set; }
        public FilterOperand<string>? Code { get; set; }
        public FilterOperand<string>? ShortName { get; set; }
        public FilterOperand<string>? ShortNameLatin { get; set; }
        public FilterOperand<object>? Depots { get; set; }
        public FilterOperand<bool>? IsCity { get; set; }
        public FilterOperand<string>? CityCode { get; set; }
        public FilterOperand<bool>? IsSalePoint { get; set; }
    }
}
