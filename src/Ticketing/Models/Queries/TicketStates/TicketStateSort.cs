using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.TicketStates
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
