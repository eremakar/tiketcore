using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;

namespace Ticketing.Tarifications.Models.Queries.Services
{
    public partial class ServiceQuery : QueryBase<Service, ServiceFilter, ServiceSort>
    {
    }
}
