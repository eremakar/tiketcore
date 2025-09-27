using Data.Repository;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketing.Data.TicketDb.Entities
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
        public Wagon? Wagon { get; set; }

        [InverseProperty("Wagon")]
        public List<Seat>? Seats { get; set; }
    }
}
