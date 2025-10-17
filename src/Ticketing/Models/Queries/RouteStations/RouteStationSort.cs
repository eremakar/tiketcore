using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.RouteStations
{
    /// <summary>
    /// Станция маршрута
    /// </summary>
    public partial class RouteStationSort : SortBase<RouteStation>
    {
        public SortOperand? Id { get; set; }
        /// <summary>
        /// Порядок следования
        /// </summary>
        public SortOperand? Order { get; set; }
        /// <summary>
        /// Время прибытия
        /// </summary>
        public SortOperand? Arrival { get; set; }
        /// <summary>
        /// Остановка
        /// </summary>
        public SortOperand? Stop { get; set; }
        /// <summary>
        /// Время отправления
        /// </summary>
        public SortOperand? Departure { get; set; }
        public SortOperand? Distance { get; set; }
        /// <summary>
        /// Сутки
        /// </summary>
        public SortOperand? Day { get; set; }
        public SortOperand? StationId { get; set; }
        public SortOperand? RouteId { get; set; }
    }
}
