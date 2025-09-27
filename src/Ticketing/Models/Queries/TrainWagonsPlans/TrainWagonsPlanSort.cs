using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.TrainWagonsPlans
{
    /// <summary>
    /// План состава поезда
    /// </summary>
    public partial class TrainWagonsPlanSort : SortBase<TrainWagonsPlan>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? Name { get; set; }
        public SortOperand? TrainId { get; set; }
    }
}
