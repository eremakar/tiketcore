﻿using Ticketing.Models;
using Api.AspNetCore.Models.Scope;
using Api.AspNetCore.Services;

namespace Ticketing.Services
{
    public class MicroserviceAuthorizeService : AuthorizeService
    {
        public MicroserviceAuthorizeService(IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
        }
    }
}
