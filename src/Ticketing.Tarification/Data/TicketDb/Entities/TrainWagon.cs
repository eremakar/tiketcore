using Data.Repository;

namespace Ticketing.Tarifications.Data.TicketDb.Entities
{
    /// <summary>
    /// Вагон состава поезда
    /// </summary>
    public partial class TrainWagon : IEntityKey<long>
    {
        public long Id { get; set; }
        public string? Number { get; set; }
        public long? TrainScheduleId { get; set; }
        public long? WagonId { get; set; }

        public TrainSchedule? TrainSchedule { get; set; }
        public WagonModel? Wagon { get; set; }
    }
}
