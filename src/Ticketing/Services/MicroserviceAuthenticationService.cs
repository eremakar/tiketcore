using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Models;
using Api.AspNetCore.Models.Configuration;
using Api.AspNetCore.Models.Secure;
using Api.AspNetCore.Services;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Ticketing.Services
{
    public class MicroserviceAuthenticationService : JwtTokenAuthenticationService
    {
        private readonly TicketDbContext db;

        public MicroserviceAuthenticationService(IUserManagementService userManagementService,
            IOptions<TokenManagement> tokenManagement,
            ILogger<MicroserviceAuthenticationService> logger,
            TicketDbContext db)
            : base(userManagementService, tokenManagement, logger)
        {
            this.db = db;
        }
    }
}
