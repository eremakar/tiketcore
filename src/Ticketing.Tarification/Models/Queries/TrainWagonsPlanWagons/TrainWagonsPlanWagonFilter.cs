using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;

namespace Ticketing.Tarifications.Models.Queries.TrainWagonsPlanWagons
{
    /// <summary>
    /// Вагон в плане состава
    /// </summary>
    public partial class TrainWagonsPlanWagonFilter : FilterBase<TrainWagonsPlanWagon>
    {
        public FilterOperand<long>? Id { get; set; }
        public FilterOperand<string>? Number { get; set; }
        public FilterOperand<long?>? PlanId { get; set; }
        public FilterOperand<long?>? WagonId { get; set; }
    }
}
