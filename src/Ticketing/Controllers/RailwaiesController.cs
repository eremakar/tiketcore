using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Mappings;
using Ticketing.Models.Dtos;
using Ticketing.Models.Queries.Railwaies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;

namespace Ticketing.Controllers
{
    /// <summary>
    /// ЖД дорога
    /// </summary>
    [Route("/api/v1/railwaies")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class RailwaiesController : RestControllerBase2<Railway, long, RailwayDto, RailwayQuery, RailwayMap>
    {
        private TicketDbContext db;

        public RailwaiesController(ILogger<RestServiceBase<Railway, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            RailwayMap railwayMap)
            : base(logger,
                restDapperDb,
                restDb,
                "Railwaies",
                railwayMap)
        {
            db = restDb;
        }

        /// <summary>
        /// Search of Railway using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of railwaies</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/railwaies/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<RailwayDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<RailwayDto>> SearchAsync([FromBody] RailwayQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.Stations).ThenInclude(_ => _.Station));
        }

        /// <summary>
        /// Get the railway by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">Railway data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/railwaies/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(RailwayDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<RailwayDto> FindAsync([FromRoute] long key)
        {
            return await FindUsingEfAsync(key, _ => _.
                Include(_ => _.Stations).ThenInclude(_ => _.Station));
        }

    }
}
