using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Mappings;
using Ticketing.Models.Dtos;
using Ticketing.Models.Queries.TrainWagons;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;
using Ticketing.Services;
using Microsoft.EntityFrameworkCore;

namespace Ticketing.Controllers
{
    /// <summary>
    /// Вагон состава поезда
    /// </summary>
    [Route("/api/v1/trainWagons")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class TrainWagonsController : RestControllerBase2<TrainWagon, long, TrainWagonDto, TrainWagonQuery, TrainWagonMap>
    {
        protected TicketDbContext db;
        private readonly TrainWagonsService trainWagonsService;

        public TrainWagonsController(ILogger<RestServiceBase<TrainWagon, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            TrainWagonMap trainWagonMap,
            TrainWagonsService trainWagonsService)
            : base(logger,
                restDapperDb,
                restDb,
                "TrainWagons",
                trainWagonMap)
        {
            db = restDb;
            this.trainWagonsService = trainWagonsService;
        }

        /// <summary>
        /// Search of TrainWagon using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of trainWagons</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/trainWagons/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<TrainWagonDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<TrainWagonDto>> SearchAsync([FromBody] TrainWagonQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.TrainSchedule).ThenInclude(_ => _.Train).
                Include(_ => _.Wagon).ThenInclude(_ => _.Type).
                Include(_ => _.Wagon).ThenInclude(_ => _.Seats));
        }

        /// <summary>
        /// Get the trainWagon by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">TrainWagon data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/trainWagons/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(TrainWagonDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<TrainWagonDto> FindAsync([FromRoute] long key)
        {
            return await FindUsingEfAsync(key, _ => _.
                Include(_ => _.TrainSchedule).ThenInclude(_ => _.Train).
                Include(_ => _.Wagon).ThenInclude(_ => _.Type).
                Include(_ => _.Wagon).ThenInclude(_ => _.Seats));
        }

    }
}
