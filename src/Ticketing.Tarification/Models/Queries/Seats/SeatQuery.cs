using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;

namespace Ticketing.Tarifications.Models.Queries.Seats
{
    public partial class SeatQuery : QueryBase<Seat, SeatFilter, SeatSort>
    {
    }
}
