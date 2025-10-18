using Data.Repository;
using Ticketing.Data.TicketDb.Entities.Dictionaries;
using Ticketing.Data.TicketDb.Entities.Tarifications;

namespace Ticketing.Data.TicketDb.Entities
{
    /// <summary>
    /// Поезд по маршруту
    /// </summary>
    public partial class Train : IEntityKey<long>
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        /// <summary>
        /// Зональность: пригородный, общий и т.п.
        /// </summary>
        public int ZoneType { get; set; }
        /// <summary>
        /// 1 - Социально-значимый, 2 - Коммерческий
        /// </summary>
        public int Importance { get; set; }
        /// <summary>
        /// 1 - вагон-ресторан, 2 - вагон-бар
        /// </summary>
        public int Amenities { get; set; }
        /// <summary>
        /// Тип поезда, например Тальго
        /// </summary>
        public long? TypeId { get; set; }
        public long? FromId { get; set; }
        public long? ToId { get; set; }
        public long? RouteId { get; set; }
        public long? PeriodicityId { get; set; }
        public long? PlanId { get; set; }
        public long? CategoryId { get; set; }
        public long? TariffId { get; set; }

        public TrainType? Type { get; set; }
        public Station? From { get; set; }
        public Station? To { get; set; }
        public Route? Route { get; set; }
        public Periodicity? Periodicity { get; set; }
        public TrainWagonsPlan? Plan { get; set; }
        public TrainCategory? Category { get; set; }
        public Tariff? Tariff { get; set; }
    }
}
