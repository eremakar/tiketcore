using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;

namespace Ticketing.Tarifications.Data.TicketDb.Entities.Tarifications
{
    /// <summary>
    /// История тарифа места в вагоне
    /// </summary>
    public partial class SeatTariffHistory : IEntityKey<long>
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public double Price { get; set; }
        public DateTime DateTime { get; set; }
        public long? BaseFareId { get; set; }
        public long? TrainId { get; set; }
        public long? TrainCategoryId { get; set; }
        public long? WagonClassId { get; set; }
        public long? SeasonId { get; set; }
        public long? SeatTypeId { get; set; }
        /// <summary>
        /// соединение станций
        /// </summary>
        public long? ConnectionId { get; set; }

        public BaseFare? BaseFare { get; set; }
        public Train? Train { get; set; }
        public TrainCategory? TrainCategory { get; set; }
        public WagonClass? WagonClass { get; set; }
        public Season? Season { get; set; }
        public SeatType? SeatType { get; set; }
        public Connection? Connection { get; set; }
    }
}
