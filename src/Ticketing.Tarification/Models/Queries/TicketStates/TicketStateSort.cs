using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;

namespace Ticketing.Tarifications.Models.Queries.TicketStates
{
    /// <summary>
    /// Статус билета
    /// </summary>
    public partial class TicketStateSort : SortBase<TicketState>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? Name { get; set; }
        public SortOperand? Code { get; set; }
    }
}
