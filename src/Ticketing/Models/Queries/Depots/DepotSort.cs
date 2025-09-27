using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.Depots
{
    /// <summary>
    /// Вокзал
    /// </summary>
    public partial class DepotSort : SortBase<Depot>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? Name { get; set; }
        public SortOperand? StationId { get; set; }
    }
}
