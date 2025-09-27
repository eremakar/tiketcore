using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.Users
{
    public partial class UserQuery : QueryBase<User, UserFilter, UserSort>
    {
    }
}
