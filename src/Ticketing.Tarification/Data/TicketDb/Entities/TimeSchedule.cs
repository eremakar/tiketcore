using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;

namespace Ticketing.Data.TicketDb.Entities
{
    /// <summary>
    /// График времени на станции
    /// </summary>
    public partial class TimeSchedule : IEntityKey<long>
    {
        public long Id { get; set; }
        public DateTime? Arrival { get; set; }
        public int Stop { get; set; }
        public DateTime? Departure { get; set; }
        public long? TrainId { get; set; }
        public long? RouteStationId { get; set; }

        public Train? Train { get; set; }
        public RouteStation? RouteStation { get; set; }
    }
}
