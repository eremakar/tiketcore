using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.Depots
{
    /// <summary>
    /// Вокзал
    /// </summary>
    public partial class DepotFilter : FilterBase<Depot>
    {
        public FilterOperand<long>? Id { get; set; }
        public FilterOperand<string>? Name { get; set; }
        public FilterOperand<long?>? StationId { get; set; }
    }
}
