using Data.Repository;
using Ticketing.Data.TicketDb.Entities.Tarifications;

namespace Ticketing.Data.TicketDb.Entities
{
    /// <summary>
    /// Расписание поезда по дням
    /// </summary>
    public partial class TrainSchedule : IEntityKey<long>
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public bool Active { get; set; }
        public long? TrainId { get; set; }
        public long? SeatTariffId { get; set; }

        public Train? Train { get; set; }
        public SeatTariff? SeatTariff { get; set; }
    }
}
