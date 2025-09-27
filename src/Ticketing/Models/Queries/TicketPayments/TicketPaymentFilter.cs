using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.TicketPayments
{
    /// <summary>
    /// Оплата билета
    /// </summary>
    public partial class TicketPaymentFilter : FilterBase<TicketPayment>
    {
        public FilterOperand<long>? Id { get; set; }
        public FilterOperand<DateTime>? Time { get; set; }
        public FilterOperand<double>? Price { get; set; }
        public FilterOperand<int>? State { get; set; }
        public FilterOperand<long?>? TicketId { get; set; }
        public FilterOperand<int?>? UserId { get; set; }
    }
}
