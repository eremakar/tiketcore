using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.TicketPayments
{
    public partial class TicketPaymentQuery : QueryBase<TicketPayment, TicketPaymentFilter, TicketPaymentSort>
    {
    }
}
