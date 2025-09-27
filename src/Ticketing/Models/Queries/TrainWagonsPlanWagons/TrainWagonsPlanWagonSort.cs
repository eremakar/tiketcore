using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.TrainWagonsPlanWagons
{
    /// <summary>
    /// Вагон в плане состава
    /// </summary>
    public partial class TrainWagonsPlanWagonSort : SortBase<TrainWagonsPlanWagon>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? Number { get; set; }
        public SortOperand? PlanId { get; set; }
        public SortOperand? WagonId { get; set; }
    }
}
