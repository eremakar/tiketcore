using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Mappings;
using Ticketing.Models.Dtos;
using Ticketing.Models.Queries.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;

namespace Ticketing.Controllers
{
    /// <summary>
    /// Сервис/услуга
    /// </summary>
    [Route("/api/v1/services")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class ServicesController : RestControllerBase2<Service, long, ServiceDto, ServiceQuery, ServiceMap>
    {
        public ServicesController(ILogger<RestServiceBase<Service, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            ServiceMap serviceMap)
            : base(logger,
                restDapperDb,
                restDb,
                "Services",
                serviceMap)
        {
        }

        /// <summary>
        /// Search of Service using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of services</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/services/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<ServiceDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<ServiceDto>> SearchAsync([FromBody] ServiceQuery query)
        {
            return await base.SearchAsync(query);
        }

        /// <summary>
        /// Get the service by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">Service data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/services/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(ServiceDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<ServiceDto> FindAsync([FromRoute] long key)
        {
            return await base.FindAsync(key);
        }

    }
}
