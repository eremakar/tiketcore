using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.TicketPayments
{
    /// <summary>
    /// Оплата билета
    /// </summary>
    public partial class TicketPaymentSort : SortBase<TicketPayment>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? Time { get; set; }
        public SortOperand? Price { get; set; }
        public SortOperand? State { get; set; }
        public SortOperand? TicketId { get; set; }
        public SortOperand? UserId { get; set; }
    }
}
