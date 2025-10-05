using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.Carriers
{
    public partial class CarrierQuery : QueryBase<Carrier, CarrierFilter, CarrierSort>
    {
    }
}
