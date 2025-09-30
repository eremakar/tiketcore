
namespace Ticketing.Tarifications.Models.Dtos
{
    /// <summary>
    /// Соединение 2х станций
    /// </summary>
    public partial class ConnectionDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public double DistanceKm { get; set; }
        public long? FromId { get; set; }
        public long? ToId { get; set; }

        public StationDto? From { get; set; }
        public StationDto? To { get; set; }
    }
}
