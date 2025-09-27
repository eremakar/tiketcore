using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Mappings;
using Ticketing.Models.Dtos;
using Ticketing.Models.Queries.TimeSchedules;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;

namespace Ticketing.Controllers
{
    /// <summary>
    /// График времени на станции
    /// </summary>
    [Route("/api/v1/timeSchedules")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class TimeSchedulesController : RestControllerBase2<TimeSchedule, long, TimeScheduleDto, TimeScheduleQuery, TimeScheduleMap>
    {
        public TimeSchedulesController(ILogger<RestServiceBase<TimeSchedule, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            TimeScheduleMap timeScheduleMap)
            : base(logger,
                restDapperDb,
                restDb,
                "TimeSchedules",
                timeScheduleMap)
        {
        }

        /// <summary>
        /// Search of TimeSchedule using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of timeSchedules</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/timeSchedules/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<TimeScheduleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<TimeScheduleDto>> SearchAsync([FromBody] TimeScheduleQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.Train).
                Include(_ => _.RouteStation));
        }

        /// <summary>
        /// Get the timeSchedule by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">TimeSchedule data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/timeSchedules/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(TimeScheduleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<TimeScheduleDto> FindAsync([FromRoute] long key)
        {
            return await FindUsingEfAsync(key, _ => _.
                Include(_ => _.Train).
                Include(_ => _.RouteStation));
        }

    }
}
