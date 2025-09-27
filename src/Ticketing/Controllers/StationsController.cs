using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Mappings;
using Ticketing.Models.Dtos;
using Ticketing.Models.Queries.Stations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;

namespace Ticketing.Controllers
{
    /// <summary>
    /// Станция
    /// </summary>
    [Route("/api/v1/stations")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class StationsController : RestControllerBase2<Station, long, StationDto, StationQuery, StationMap>
    {
        public StationsController(ILogger<RestServiceBase<Station, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            StationMap stationMap)
            : base(logger,
                restDapperDb,
                restDb,
                "Stations",
                stationMap)
        {
        }

        /// <summary>
        /// Search of Station using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of stations</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/stations/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<StationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<StationDto>> SearchAsync([FromBody] StationQuery query)
        {
            return await base.SearchAsync(query);
        }

        /// <summary>
        /// Get the station by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">Station data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/stations/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(StationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<StationDto> FindAsync([FromRoute] long key)
        {
            return await base.FindAsync(key);
        }

    }
}
