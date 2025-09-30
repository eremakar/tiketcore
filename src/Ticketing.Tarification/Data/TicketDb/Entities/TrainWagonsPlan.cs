using Data.Repository;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketing.Tarifications.Data.TicketDb.Entities
{
    /// <summary>
    /// План состава поезда
    /// </summary>
    public partial class TrainWagonsPlan : IEntityKey<long>
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public long? TrainId { get; set; }

        public Train? Train { get; set; }

        [InverseProperty("Plan")]
        public List<TrainWagonsPlanWagon>? Wagons { get; set; }
    }
}
