using HotChocolate;
using HotChocolate.Authorization;
using Ticketing.Models.Dtos;

namespace Ticketing.Services.GraphQL
{
    public class UsersService : RestService2<User, int, UserDto, UserQuery, UserMap>
    {
        private readonly TicketDbContext db;

        public UsersService(ILogger<RestServiceBase<User, int>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            UserMap map)
            : base(logger,
                restDapperDb,
                restDb,
                "Users",
                map)
        {
            this.db = restDb;
        }

        public override async Task<PagedList<UserDto>> SearchAsync(UserQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.Role).
                Include(_ => _.Roles));
        }
    }
}
