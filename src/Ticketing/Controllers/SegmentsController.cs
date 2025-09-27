using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Mappings;
using Ticketing.Models.Dtos;
using Ticketing.Models.Queries.Segments;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;

namespace Ticketing.Controllers
{
    /// <summary>
    /// Сегмент поездки (от-до)
    /// </summary>
    [Route("/api/v1/segments")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class SegmentsController : RestControllerBase2<Segment, long, SegmentDto, SegmentQuery, SegmentMap>
    {
        public SegmentsController(ILogger<RestServiceBase<Segment, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            SegmentMap segmentMap)
            : base(logger,
                restDapperDb,
                restDb,
                "Segments",
                segmentMap)
        {
        }

        /// <summary>
        /// Search of Segment using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of segments</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/segments/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<SegmentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<SegmentDto>> SearchAsync([FromBody] SegmentQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.Seat).
                Include(_ => _.From).
                Include(_ => _.To).
                Include(_ => _.Train).
                Include(_ => _.Wagon).
                Include(_ => _.TrainSchedule).
                Include(_ => _.Ticket).
                Include(_ => _.Reservation));
        }

        /// <summary>
        /// Get the segment by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">Segment data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/segments/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(SegmentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<SegmentDto> FindAsync([FromRoute] long key)
        {
            return await FindUsingEfAsync(key, _ => _.
                Include(_ => _.Seat).
                Include(_ => _.From).
                Include(_ => _.To).
                Include(_ => _.Train).
                Include(_ => _.Wagon).
                Include(_ => _.TrainSchedule).
                Include(_ => _.Ticket).
                Include(_ => _.Reservation));
        }

    }
}
