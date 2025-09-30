using Data.Repository;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketing.Tarifications.Data.TicketDb.Entities
{
    /// <summary>
    /// Маршрут
    /// </summary>
    public partial class Route : IEntityKey<long>
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        /// <summary>
        /// Номер поезда
        /// </summary>
        public long? TrainId { get; set; }

        public Train? Train { get; set; }

        [InverseProperty("Route")]
        public List<RouteStation>? Stations { get; set; }
    }
}
