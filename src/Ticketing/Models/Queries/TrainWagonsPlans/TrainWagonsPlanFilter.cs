using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.TrainWagonsPlans
{
    /// <summary>
    /// План состава поезда
    /// </summary>
    public partial class TrainWagonsPlanFilter : FilterBase<TrainWagonsPlan>
    {
        public FilterOperand<long>? Id { get; set; }
        public FilterOperand<string>? Name { get; set; }
        public FilterOperand<long?>? TrainId { get; set; }
    }
}
