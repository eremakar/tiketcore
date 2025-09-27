using HotChocolate;
using HotChocolate.Authorization;
using Ticketing.Models.Dtos;

namespace Ticketing.Services.GraphQL
{
    public class RolesService : RestService2<Role, int, RoleDto, RoleQuery, RoleMap>
    {
        private readonly TicketDbContext db;

        public RolesService(ILogger<RestServiceBase<Role, int>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            RoleMap map)
            : base(logger,
                restDapperDb,
                restDb,
                "Roles",
                map)
        {
            this.db = restDb;
        }

        public override async Task<PagedList<RoleDto>> SearchAsync(RoleQuery query)
        {
            return await base.SearchAsync(query);
        }
    }
}
