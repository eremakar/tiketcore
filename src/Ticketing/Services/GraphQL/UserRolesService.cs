using HotChocolate;
using HotChocolate.Authorization;
using Ticketing.Models.Dtos;

namespace Ticketing.Services.GraphQL
{
    public class UserRolesService : RestService2<UserRole, int, UserRoleDto, UserRoleQuery, UserRoleMap>
    {
        private readonly TicketDbContext db;

        public UserRolesService(ILogger<RestServiceBase<UserRole, int>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            UserRoleMap map)
            : base(logger,
                restDapperDb,
                restDb,
                "UserRoles",
                map)
        {
            this.db = restDb;
        }

        public override async Task<PagedList<UserRoleDto>> SearchAsync(UserRoleQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.User).
                Include(_ => _.Role));
        }
    }
}
