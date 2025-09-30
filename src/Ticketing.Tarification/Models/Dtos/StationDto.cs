
namespace Ticketing.Tarifications.Models.Dtos
{
    /// <summary>
    /// Станция
    /// </summary>
    public partial class StationDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? ShortName { get; set; }
        public string? ShortNameLatin { get; set; }
        public object? Depots { get; set; }
        public bool IsCity { get; set; }
        public string? CityCode { get; set; }
        public bool IsSalePoint { get; set; }
    }
}
