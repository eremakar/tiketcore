using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Mappings;
using Ticketing.Models.Dtos;
using Ticketing.Models.Queries.SeatSegments;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;

namespace Ticketing.Controllers
{
    /// <summary>
    /// Сегмент по месту (от-до)
    /// </summary>
    [Route("/api/v1/seatSegments")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class SeatSegmentsController : RestControllerBase2<SeatSegment, long, SeatSegmentDto, SeatSegmentQuery, SeatSegmentMap>
    {
        public SeatSegmentsController(ILogger<RestServiceBase<SeatSegment, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            SeatSegmentMap seatSegmentMap)
            : base(logger,
                restDapperDb,
                restDb,
                "SeatSegments",
                seatSegmentMap)
        {
        }

        /// <summary>
        /// Search of SeatSegment using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of seatSegments</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/seatSegments/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<SeatSegmentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<SeatSegmentDto>> SearchAsync([FromBody] SeatSegmentQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.Seat).
                Include(_ => _.From).ThenInclude(_ => _.Station).
                Include(_ => _.To).ThenInclude(_ => _.Station).
                Include(_ => _.Train).
                Include(_ => _.Wagon).
                Include(_ => _.TrainSchedule).
                Include(_ => _.Ticket).
                Include(_ => _.SeatReservation));
        }

        /// <summary>
        /// Get the seatSegment by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">SeatSegment data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/seatSegments/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(SeatSegmentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<SeatSegmentDto> FindAsync([FromRoute] long key)
        {
            return await FindUsingEfAsync(key, _ => _.
                Include(_ => _.Seat).
                Include(_ => _.From).ThenInclude(_ => _.Station).
                Include(_ => _.To).ThenInclude(_ => _.Station).
                Include(_ => _.Train).
                Include(_ => _.Wagon).
                Include(_ => _.TrainSchedule).
                Include(_ => _.Ticket).
                Include(_ => _.SeatReservation));
        }

    }
}
