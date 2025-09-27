using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.Services
{
    public partial class ServiceQuery : QueryBase<Service, ServiceFilter, ServiceSort>
    {
    }
}
