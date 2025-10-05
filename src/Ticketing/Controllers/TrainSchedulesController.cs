using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Mappings;
using Ticketing.Models.Dtos;
using Ticketing.Models.Queries.TrainSchedules;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;
using Ticketing.Services;

namespace Ticketing.Controllers
{
    /// <summary>
    /// Расписание поезда по дням
    /// </summary>
    [Route("/api/v1/trainSchedules")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class TrainSchedulesController : RestControllerBase2<TrainSchedule, long, TrainScheduleDto, TrainScheduleQuery, TrainScheduleMap>
    {
        private readonly TrainSchedulesService trainSchedulesService;

        public TrainSchedulesController(ILogger<RestServiceBase<TrainSchedule, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            TrainScheduleMap trainScheduleMap,
            TrainSchedulesService trainSchedulesService)
            : base(logger,
                restDapperDb,
                restDb,
                "TrainSchedules",
                trainScheduleMap)
        {
            this.trainSchedulesService = trainSchedulesService;
        }

        /// <summary>
        /// Search of TrainSchedule using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of trainSchedules</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/trainSchedules/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<TrainScheduleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<TrainScheduleDto>> SearchAsync([FromBody] TrainScheduleQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.Train).
                Include(_ => _.SeatTariff));
        }

        /// <summary>
        /// Get the trainSchedule by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">TrainSchedule data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/trainSchedules/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(TrainScheduleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<TrainScheduleDto> FindAsync([FromRoute] long key)
        {
            return await FindUsingEfAsync(key, _ => _.
                Include(_ => _.Train).
                Include(_ => _.SeatTariff));
        }

    }
}
