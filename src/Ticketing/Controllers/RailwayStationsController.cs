using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Mappings;
using Ticketing.Models.Dtos;
using Ticketing.Models.Queries.RailwayStations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;

namespace Ticketing.Controllers
{
    /// <summary>
    /// Станция дороги
    /// </summary>
    [Route("/api/v1/railwayStations")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class RailwayStationsController : RestControllerBase2<RailwayStation, long, RailwayStationDto, RailwayStationQuery, RailwayStationMap>
    {
        public RailwayStationsController(ILogger<RestServiceBase<RailwayStation, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            RailwayStationMap railwayStationMap)
            : base(logger,
                restDapperDb,
                restDb,
                "RailwayStations",
                railwayStationMap)
        {
        }

        /// <summary>
        /// Search of RailwayStation using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of railwayStations</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/railwayStations/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<RailwayStationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<RailwayStationDto>> SearchAsync([FromBody] RailwayStationQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.Station).
                Include(_ => _.Railway));
        }

        /// <summary>
        /// Get the railwayStation by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">RailwayStation data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/railwayStations/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(RailwayStationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<RailwayStationDto> FindAsync([FromRoute] long key)
        {
            return await FindUsingEfAsync(key, _ => _.
                Include(_ => _.Station).
                Include(_ => _.Railway));
        }

    }
}
