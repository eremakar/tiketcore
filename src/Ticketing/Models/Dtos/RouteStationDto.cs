
namespace Ticketing.Models.Dtos
{
    /// <summary>
    /// Станция маршрута
    /// </summary>
    public partial class RouteStationDto
    {
        public long Id { get; set; }
        /// <summary>
        /// Порядок следования
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// Время прибытия
        /// </summary>
        public DateTime? Arrival { get; set; }
        /// <summary>
        /// Остановка
        /// </summary>
        public DateTime? Stop { get; set; }
        /// <summary>
        /// Время отправления
        /// </summary>
        public DateTime? Departure { get; set; }
        public double Distance { get; set; }
        /// <summary>
        /// Сутки
        /// </summary>
        public int Day { get; set; }
        public long? StationId { get; set; }
        public long? RouteId { get; set; }

        public StationDto? Station { get; set; }
        public RouteDto? Route { get; set; }
    }
}
