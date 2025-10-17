using Data.Repository;

namespace Ticketing.Data.TicketDb.Entities
{
    /// <summary>
    /// Станция маршрута
    /// </summary>
    public partial class RouteStation : IEntityKey<long>
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
        public long? StationId { get; set; }
        public long? RouteId { get; set; }
        /// <summary>
        /// Сутки
        /// </summary>
        public int Day { get; set; }

        public Station? Station { get; set; }
        public Route? Route { get; set; }
    }
}
