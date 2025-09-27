using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Mappings;
using Ticketing.Models.Dtos;
using Ticketing.Models.Queries.TrainWagonsPlanWagons;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;

namespace Ticketing.Controllers
{
    /// <summary>
    /// Вагон в плане состава
    /// </summary>
    [Route("/api/v1/trainWagonsPlanWagons")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class TrainWagonsPlanWagonsController : RestControllerBase2<TrainWagonsPlanWagon, long, TrainWagonsPlanWagonDto, TrainWagonsPlanWagonQuery, TrainWagonsPlanWagonMap>
    {
        public TrainWagonsPlanWagonsController(ILogger<RestServiceBase<TrainWagonsPlanWagon, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            TrainWagonsPlanWagonMap trainWagonsPlanWagonMap)
            : base(logger,
                restDapperDb,
                restDb,
                "TrainWagonsPlanWagons",
                trainWagonsPlanWagonMap)
        {
        }

        /// <summary>
        /// Search of TrainWagonsPlanWagon using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of trainWagonsPlanWagons</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/trainWagonsPlanWagons/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<TrainWagonsPlanWagonDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<TrainWagonsPlanWagonDto>> SearchAsync([FromBody] TrainWagonsPlanWagonQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.Plan).
                Include(_ => _.Wagon).ThenInclude(_ => _.Type));
        }

        /// <summary>
        /// Get the trainWagonsPlanWagon by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">TrainWagonsPlanWagon data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/trainWagonsPlanWagons/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(TrainWagonsPlanWagonDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<TrainWagonsPlanWagonDto> FindAsync([FromRoute] long key)
        {
            return await FindUsingEfAsync(key, _ => _.
                Include(_ => _.Plan).
                Include(_ => _.Wagon).ThenInclude(_ => _.Type));
        }

    }
}
