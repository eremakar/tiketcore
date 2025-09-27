using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Mappings;
using Ticketing.Models.Dtos;
using Ticketing.Models.Queries.SeatCountSegments;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;

namespace Ticketing.Controllers
{
    /// <summary>
    /// Сегмент по количеству мест
    /// </summary>
    [Route("/api/v1/seatCountSegments")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class SeatCountSegmentsController : RestControllerBase2<SeatCountSegment, long, SeatCountSegmentDto, SeatCountSegmentQuery, SeatCountSegmentMap>
    {
        public SeatCountSegmentsController(ILogger<RestServiceBase<SeatCountSegment, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            SeatCountSegmentMap seatCountSegmentMap)
            : base(logger,
                restDapperDb,
                restDb,
                "SeatCountSegments",
                seatCountSegmentMap)
        {
        }

        /// <summary>
        /// Search of SeatCountSegment using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of seatCountSegments</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/seatCountSegments/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<SeatCountSegmentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<SeatCountSegmentDto>> SearchAsync([FromBody] SeatCountSegmentQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.From).
                Include(_ => _.To).
                Include(_ => _.Train).
                Include(_ => _.Wagon).
                Include(_ => _.TrainSchedule).
                Include(_ => _.SeatCountReservation));
        }

        /// <summary>
        /// Get the seatCountSegment by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">SeatCountSegment data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/seatCountSegments/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(SeatCountSegmentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<SeatCountSegmentDto> FindAsync([FromRoute] long key)
        {
            return await FindUsingEfAsync(key, _ => _.
                Include(_ => _.From).
                Include(_ => _.To).
                Include(_ => _.Train).
                Include(_ => _.Wagon).
                Include(_ => _.TrainSchedule).
                Include(_ => _.SeatCountReservation));
        }

    }
}
