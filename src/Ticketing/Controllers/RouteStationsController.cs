using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Mappings;
using Ticketing.Models.Dtos;
using Ticketing.Models.Queries.RouteStations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;

namespace Ticketing.Controllers
{
    /// <summary>
    /// Станция маршрута
    /// </summary>
    [Route("/api/v1/routeStations")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class RouteStationsController : RestControllerBase2<RouteStation, long, RouteStationDto, RouteStationQuery, RouteStationMap>
    {
        public RouteStationsController(ILogger<RestServiceBase<RouteStation, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            RouteStationMap routeStationMap)
            : base(logger,
                restDapperDb,
                restDb,
                "RouteStations",
                routeStationMap)
        {
        }

        /// <summary>
        /// Search of RouteStation using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of routeStations</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/routeStations/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<RouteStationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<RouteStationDto>> SearchAsync([FromBody] RouteStationQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.Station).
                Include(_ => _.Route));
        }

        /// <summary>
        /// Get the routeStation by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">RouteStation data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/routeStations/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(RouteStationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<RouteStationDto> FindAsync([FromRoute] long key)
        {
            return await FindUsingEfAsync(key, _ => _.
                Include(_ => _.Station).
                Include(_ => _.Route));
        }

    }
}
