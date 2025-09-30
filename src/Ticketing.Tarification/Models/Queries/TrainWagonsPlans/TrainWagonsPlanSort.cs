using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;

namespace Ticketing.Tarifications.Models.Queries.TrainWagonsPlans
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
