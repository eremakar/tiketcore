using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;

namespace Ticketing.Tarifications.Models.Queries.TicketPayments
{
    public partial class TicketPaymentQuery : QueryBase<TicketPayment, TicketPaymentFilter, TicketPaymentSort>
    {
    }
}
