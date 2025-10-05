using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.TrainWagons
{
    /// <summary>
    /// Вагон состава поезда
    /// </summary>
    public partial class TrainWagonFilter : FilterBase<TrainWagon>
    {
        public FilterOperand<long>? Id { get; set; }
        public FilterOperand<string>? Number { get; set; }
        public FilterOperand<long?>? TrainScheduleId { get; set; }
        public FilterOperand<long?>? WagonId { get; set; }
        public FilterOperand<long?>? CarrierId { get; set; }
    }
}
