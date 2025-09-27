
namespace Ticketing.Models.Dtos
{
    /// <summary>
    /// График времени на станции
    /// </summary>
    public partial class TimeScheduleDto
    {
        public long Id { get; set; }
        public DateTime? Arrival { get; set; }
        public int Stop { get; set; }
        public DateTime? Departure { get; set; }
        public long? TrainId { get; set; }
        public long? RouteStationId { get; set; }

        public TrainDto? Train { get; set; }
        public RouteStationDto? RouteStation { get; set; }
    }
}
