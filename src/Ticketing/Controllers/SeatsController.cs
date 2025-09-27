using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Mappings;
using Ticketing.Models.Dtos;
using Ticketing.Models.Queries.Seats;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;

namespace Ticketing.Controllers
{
    /// <summary>
    /// Место в вагоне
    /// </summary>
    [Route("/api/v1/seats")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class SeatsController : RestControllerBase2<Seat, long, SeatDto, SeatQuery, SeatMap>
    {
        public SeatsController(ILogger<RestServiceBase<Seat, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            SeatMap seatMap)
            : base(logger,
                restDapperDb,
                restDb,
                "Seats",
                seatMap)
        {
        }

        /// <summary>
        /// Search of Seat using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of seats</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/seats/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<SeatDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<SeatDto>> SearchAsync([FromBody] SeatQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.Wagon).
                Include(_ => _.Type));
        }

        /// <summary>
        /// Get the seat by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">Seat data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/seats/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(SeatDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<SeatDto> FindAsync([FromRoute] long key)
        {
            return await FindUsingEfAsync(key, _ => _.
                Include(_ => _.Wagon).
                Include(_ => _.Type));
        }

    }
}
