using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;

namespace Ticketing.Tarifications.Models.Queries.Users
{
    public partial class UserQuery : QueryBase<User, UserFilter, UserSort>
    {
    }
}
