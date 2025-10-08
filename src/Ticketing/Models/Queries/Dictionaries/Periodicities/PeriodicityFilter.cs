using Data.Repository;
using Ticketing.Data.TicketDb.Entities.Dictionaries;

namespace Ticketing.Models.Queries.Dictionaries.Periodicities
{
    /// <summary>
    /// Периодичность
    /// </summary>
    public partial class PeriodicityFilter : FilterBase<Periodicity>
    {
        public FilterOperand<long>? Id { get; set; }
        public FilterOperand<string>? Name { get; set; }
        public FilterOperand<string>? Code { get; set; }
    }
}
