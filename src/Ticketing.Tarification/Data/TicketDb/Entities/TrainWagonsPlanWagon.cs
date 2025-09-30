using Data.Repository;

namespace Ticketing.Tarifications.Data.TicketDb.Entities
{
    /// <summary>
    /// Вагон в плане состава
    /// </summary>
    public partial class TrainWagonsPlanWagon : IEntityKey<long>
    {
        public long Id { get; set; }
        public string? Number { get; set; }
        public long? PlanId { get; set; }
        public long? WagonId { get; set; }

        public TrainWagonsPlan? Plan { get; set; }
        public Wagon? Wagon { get; set; }
    }
}
