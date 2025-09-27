
namespace Ticketing.Models.Dtos
{
    /// <summary>
    /// Вокзал
    /// </summary>
    public partial class DepotDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public long? StationId { get; set; }

        public StationDto? Station { get; set; }
    }
}
