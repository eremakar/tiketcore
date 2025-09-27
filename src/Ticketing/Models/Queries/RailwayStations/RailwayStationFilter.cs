using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.RailwayStations
{
    /// <summary>
    /// Станция дороги
    /// </summary>
    public partial class RailwayStationFilter : FilterBase<RailwayStation>
    {
        public FilterOperand<long>? Id { get; set; }
        public FilterOperand<int>? Order { get; set; }
        public FilterOperand<double>? Distance { get; set; }
        public FilterOperand<long?>? StationId { get; set; }
        public FilterOperand<long?>? RailwayId { get; set; }
    }
}
