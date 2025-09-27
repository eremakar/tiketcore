using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.Routes
{
    /// <summary>
    /// Маршрут
    /// </summary>
    public partial class RouteSort : SortBase<Data.TicketDb.Entities.Route>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? Name { get; set; }
        /// <summary>
        /// Номер поезда
        /// </summary>
        public SortOperand? TrainId { get; set; }
    }
}
