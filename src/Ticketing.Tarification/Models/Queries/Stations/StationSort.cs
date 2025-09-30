using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;

namespace Ticketing.Tarifications.Models.Queries.Stations
{
    /// <summary>
    /// Станция
    /// </summary>
    public partial class StationSort : SortBase<Station>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? Name { get; set; }
        public SortOperand? Code { get; set; }
        public SortOperand? ShortName { get; set; }
        public SortOperand? ShortNameLatin { get; set; }
        public SortOperand? Depots { get; set; }
        public SortOperand? IsCity { get; set; }
        public SortOperand? CityCode { get; set; }
        public SortOperand? IsSalePoint { get; set; }
    }
}
