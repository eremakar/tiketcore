using Ticketing.Data.TicketDb.Entities;
using Ticketing.Models.Queries.Users;
using Data.Repository;

namespace Ticketing.Services
{
    public interface IUserService
    {
        Task<User> FindByUserName(string userName);
        Task<PagedList<User>> SearchAsync(UserQuery query);
        Task<User> FindAsync(int id);
        Task<bool> AddAsync(User user, bool encryptPassword = true);
        Task<bool> UpdateAsync(User user);
        Task<bool> RemoveAsync(int id);

        Task SaveChangesAsync();
    }
}
