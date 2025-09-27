using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.SeatReservations
{
    public partial class SeatReservationQuery : QueryBase<SeatReservation, SeatReservationFilter, SeatReservationSort>
    {
    }
}
