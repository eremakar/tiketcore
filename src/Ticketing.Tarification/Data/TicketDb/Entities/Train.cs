using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities.Tarifications;

namespace Ticketing.Tarifications.Data.TicketDb.Entities
{
    /// <summary>
    /// Поезд
    /// </summary>
    public partial class Train : IEntityKey<long>
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        /// <summary>
        /// Тип поезда, например Тальго
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// Зональность: пригородный, общий и т.п.
        /// </summary>
        public int ZoneType { get; set; }
        public long? FromId { get; set; }
        public long? ToId { get; set; }
        public long? RouteId { get; set; }
        public long? PlanId { get; set; }
        public long? CategoryId { get; set; }

        public Station? From { get; set; }
        public Station? To { get; set; }
        public Route? Route { get; set; }
        public TrainWagonsPlan? Plan { get; set; }
        public TrainCategory? Category { get; set; }
    }
}
