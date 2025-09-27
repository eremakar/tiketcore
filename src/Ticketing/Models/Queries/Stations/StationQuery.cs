using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.Stations
{
    public partial class StationQuery : QueryBase<Station, StationFilter, StationSort>
    {
    }
}
