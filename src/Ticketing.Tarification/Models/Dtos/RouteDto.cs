
namespace Ticketing.Tarifications.Models.Dtos
{
    /// <summary>
    /// Маршрут
    /// </summary>
    public partial class RouteDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        /// <summary>
        /// Номер поезда
        /// </summary>
        public long? TrainId { get; set; }

        /// <summary>
        /// Номер поезда
        /// </summary>
        public TrainDto? Train { get; set; }

        /// <summary>
        /// Станции маршрута
        /// </summary>
        public List<RouteStationDto>? Stations { get; set; }
    }
}
