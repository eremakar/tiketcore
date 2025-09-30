using Ticketing.Tarifications.Models;
using Api.AspNetCore.Models.Scope;
using Api.AspNetCore.Services;

namespace Ticketing.Tarifications.Services
{
    public class MicroserviceAuthorizeService : AuthorizeService
    {
        public MicroserviceAuthorizeService(IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
        }
    }
}
