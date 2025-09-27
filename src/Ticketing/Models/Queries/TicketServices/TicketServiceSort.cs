using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.TicketServices
{
    /// <summary>
    /// Услуги билета
    /// </summary>
    public partial class TicketServiceSort : SortBase<TicketService>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? State { get; set; }
        public SortOperand? TicketId { get; set; }
        public SortOperand? ServiceId { get; set; }
    }
}
