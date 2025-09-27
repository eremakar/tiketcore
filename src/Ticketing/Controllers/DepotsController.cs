using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Mappings;
using Ticketing.Models.Dtos;
using Ticketing.Models.Queries.Depots;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;

namespace Ticketing.Controllers
{
    /// <summary>
    /// Вокзал
    /// </summary>
    [Route("/api/v1/depots")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class DepotsController : RestControllerBase2<Depot, long, DepotDto, DepotQuery, DepotMap>
    {
        public DepotsController(ILogger<RestServiceBase<Depot, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            DepotMap depotMap)
            : base(logger,
                restDapperDb,
                restDb,
                "Depots",
                depotMap)
        {
        }

        /// <summary>
        /// Search of Depot using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of depots</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/depots/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<DepotDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<DepotDto>> SearchAsync([FromBody] DepotQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.Station));
        }

        /// <summary>
        /// Get the depot by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">Depot data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/depots/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(DepotDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<DepotDto> FindAsync([FromRoute] long key)
        {
            return await FindUsingEfAsync(key, _ => _.
                Include(_ => _.Station));
        }

    }
}
