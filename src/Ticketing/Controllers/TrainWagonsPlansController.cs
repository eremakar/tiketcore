using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Mappings;
using Ticketing.Models.Dtos;
using Ticketing.Models.Queries.TrainWagonsPlans;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;

namespace Ticketing.Controllers
{
    /// <summary>
    /// План состава поезда
    /// </summary>
    [Route("/api/v1/trainWagonsPlans")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class TrainWagonsPlansController : RestControllerBase2<TrainWagonsPlan, long, TrainWagonsPlanDto, TrainWagonsPlanQuery, TrainWagonsPlanMap>
    {
        public TrainWagonsPlansController(ILogger<RestServiceBase<TrainWagonsPlan, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            TrainWagonsPlanMap trainWagonsPlanMap)
            : base(logger,
                restDapperDb,
                restDb,
                "TrainWagonsPlans",
                trainWagonsPlanMap)
        {
        }

        /// <summary>
        /// Search of TrainWagonsPlan using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of trainWagonsPlans</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/trainWagonsPlans/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<TrainWagonsPlanDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<TrainWagonsPlanDto>> SearchAsync([FromBody] TrainWagonsPlanQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.Train).
                Include(_ => _.Wagons).ThenInclude(_ => _.Wagon).ThenInclude(_ => _.Type));
        }

        /// <summary>
        /// Get the trainWagonsPlan by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">TrainWagonsPlan data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/trainWagonsPlans/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(TrainWagonsPlanDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<TrainWagonsPlanDto> FindAsync([FromRoute] long key)
        {
            return await FindUsingEfAsync(key, _ => _.
                Include(_ => _.Train).
                Include(_ => _.Wagons).ThenInclude(_ => _.Wagon).ThenInclude(_ => _.Type));
        }

    }
}
