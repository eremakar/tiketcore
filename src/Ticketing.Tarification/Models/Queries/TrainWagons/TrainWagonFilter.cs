using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;

namespace Ticketing.Tarifications.Models.Queries.TrainWagons
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
    }
}
