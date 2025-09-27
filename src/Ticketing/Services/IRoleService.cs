using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Services
{
    public interface IRoleService
    {
        Task<Role> ByUserName(string userName);
    }
}
