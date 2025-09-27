using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Mappings;
using Ticketing.Models.Dtos;
using Ticketing.Models.Queries.Wagons;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;

namespace Ticketing.Controllers
{
    /// <summary>
    /// Вагон (тип)
    /// </summary>
    [Route("/api/v1/wagons")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class WagonsController : RestControllerBase2<Wagon, long, WagonDto, WagonQuery, WagonMap>
    {
        public WagonsController(ILogger<RestServiceBase<Wagon, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            WagonMap wagonMap)
            : base(logger,
                restDapperDb,
                restDb,
                "Wagons",
                wagonMap)
        {
        }

        /// <summary>
        /// Search of Wagon using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of wagons</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/wagons/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<WagonDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<WagonDto>> SearchAsync([FromBody] WagonQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.Type));
        }

        /// <summary>
        /// Get the wagon by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">Wagon data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/wagons/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(WagonDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<WagonDto> FindAsync([FromRoute] long key)
        {
            return await FindUsingEfAsync(key, _ => _.
                Include(_ => _.Type));
        }

    }
}
