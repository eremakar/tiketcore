
namespace Ticketing.Models.Dtos
{
    /// <summary>
    /// ЖД дорога
    /// </summary>
    public partial class RailwayDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? ShortCode { get; set; }
        public int TimeDifferenceFromAdministration { get; set; }
        public string? Type { get; set; }

        public List<RailwayStationDto>? Stations { get; set; }
    }
}
