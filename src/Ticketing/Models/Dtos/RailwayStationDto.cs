
namespace Ticketing.Models.Dtos
{
    /// <summary>
    /// Станция дороги
    /// </summary>
    public partial class RailwayStationDto
    {
        public long Id { get; set; }
        public int Order { get; set; }
        public double Distance { get; set; }
        public long? StationId { get; set; }
        public long? RailwayId { get; set; }

        public StationDto? Station { get; set; }
        public RailwayDto? Railway { get; set; }
    }
}
