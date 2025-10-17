using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.RouteStations
{
    /// <summary>
    /// Станция маршрута
    /// </summary>
    public partial class RouteStationFilter : FilterBase<RouteStation>
    {
        public FilterOperand<long>? Id { get; set; }
        /// <summary>
        /// Порядок следования
        /// </summary>
        public FilterOperand<int>? Order { get; set; }
        /// <summary>
        /// Время прибытия
        /// </summary>
        public FilterOperand<DateTime?>? Arrival { get; set; }
        /// <summary>
        /// Остановка
        /// </summary>
        public FilterOperand<DateTime?>? Stop { get; set; }
        /// <summary>
        /// Время отправления
        /// </summary>
        public FilterOperand<DateTime?>? Departure { get; set; }
        public FilterOperand<double>? Distance { get; set; }
        /// <summary>
        /// Сутки
        /// </summary>
        public FilterOperand<int>? Day { get; set; }
        public FilterOperand<long?>? StationId { get; set; }
        public FilterOperand<long?>? RouteId { get; set; }
    }
}
