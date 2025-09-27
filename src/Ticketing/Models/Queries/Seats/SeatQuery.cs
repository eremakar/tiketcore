using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.Seats
{
    public partial class SeatQuery : QueryBase<Seat, SeatFilter, SeatSort>
    {
    }
}
