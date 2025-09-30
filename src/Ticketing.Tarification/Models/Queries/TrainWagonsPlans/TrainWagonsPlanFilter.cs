using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;

namespace Ticketing.Tarifications.Models.Queries.TrainWagonsPlans
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
