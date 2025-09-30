using Ticketing.Tarifications.Models;
using Api.AspNetCore.Services;

namespace Ticketing.Tarifications.Services
{
    public interface IMicroserviceAuthorizeService : IAuthorizeService
    {
        Task<MicroserviceAuthorizationData> AuthorizeData(Action<MicroserviceAuthorizationData> action);
    }
}
