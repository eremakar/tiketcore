using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Mappings;
using Ticketing.Models.Dtos;
using Ticketing.Models.Queries.Routes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;

namespace Ticketing.Controllers
{
    /// <summary>
    /// Маршрут
    /// </summary>
    [Route("/api/v1/routes")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class RoutesController : RestControllerBase2<Data.TicketDb.Entities.Route, long, RouteDto, RouteQuery, RouteMap>
    {
        public RoutesController(ILogger<RestServiceBase<Data.TicketDb.Entities.Route, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            RouteMap routeMap)
            : base(logger,
                restDapperDb,
                restDb,
                "Routes",
                routeMap)
        {
        }

        /// <summary>
        /// Search of Route using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of routes</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/routes/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<RouteDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<RouteDto>> SearchAsync([FromBody] RouteQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.Train).
                Include(_ => _.Stations).ThenInclude(_ => _.Station));
        }

        /// <summary>
        /// Get the route by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">Route data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/routes/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(RouteDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<RouteDto> FindAsync([FromRoute] long key)
        {
            return await FindUsingEfAsync(key, _ => _.
                Include(_ => _.Train).
                Include(_ => _.Stations).ThenInclude(_ => _.Station));
        }

    }
}
