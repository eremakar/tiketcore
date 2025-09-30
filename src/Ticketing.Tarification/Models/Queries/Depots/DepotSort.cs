using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;

namespace Ticketing.Tarifications.Models.Queries.Depots
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
