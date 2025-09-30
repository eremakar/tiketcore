using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;

namespace Ticketing.Tarifications.Models.Queries.Routes
{
    /// <summary>
    /// Маршрут
    /// </summary>
    public partial class RouteFilter : FilterBase<Data.TicketDb.Entities.Route>
    {
        public FilterOperand<long>? Id { get; set; }
        public FilterOperand<string>? Name { get; set; }
        /// <summary>
        /// Номер поезда
        /// </summary>
        public FilterOperand<long?>? TrainId { get; set; }
    }
}
