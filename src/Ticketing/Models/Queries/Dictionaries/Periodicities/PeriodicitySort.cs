using Data.Repository;
using Ticketing.Data.TicketDb.Entities.Dictionaries;

namespace Ticketing.Models.Queries.Dictionaries.Periodicities
{
    /// <summary>
    /// Периодичность
    /// </summary>
    public partial class PeriodicitySort : SortBase<Periodicity>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? Name { get; set; }
        public SortOperand? Code { get; set; }
    }
}
