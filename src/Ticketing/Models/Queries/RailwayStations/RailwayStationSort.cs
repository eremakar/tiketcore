using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.RailwayStations
{
    /// <summary>
    /// Станция дороги
    /// </summary>
    public partial class RailwayStationSort : SortBase<RailwayStation>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? Order { get; set; }
        public SortOperand? Distance { get; set; }
        public SortOperand? StationId { get; set; }
        public SortOperand? RailwayId { get; set; }
    }
}
