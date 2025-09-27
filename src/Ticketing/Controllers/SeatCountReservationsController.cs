using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Mappings;
using Ticketing.Models.Dtos;
using Ticketing.Models.Queries.SeatCountReservations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;

namespace Ticketing.Controllers
{
    /// <summary>
    /// Бронирование количества мест
    /// </summary>
    [Route("/api/v1/seatCountReservations")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class SeatCountReservationsController : RestControllerBase2<SeatCountReservation, long, SeatCountReservationDto, SeatCountReservationQuery, SeatCountReservationMap>
    {
        public SeatCountReservationsController(ILogger<RestServiceBase<SeatCountReservation, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            SeatCountReservationMap seatCountReservationMap)
            : base(logger,
                restDapperDb,
                restDb,
                "SeatCountReservations",
                seatCountReservationMap)
        {
        }

        /// <summary>
        /// Search of SeatCountReservation using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of seatCountReservations</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/seatCountReservations/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<SeatCountReservationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<SeatCountReservationDto>> SearchAsync([FromBody] SeatCountReservationQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.From).
                Include(_ => _.To).
                Include(_ => _.Train).
                Include(_ => _.Wagon).
                Include(_ => _.TrainSchedule).
                Include(_ => _.Segments));
        }

        /// <summary>
        /// Get the seatCountReservation by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">SeatCountReservation data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/seatCountReservations/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(SeatCountReservationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<SeatCountReservationDto> FindAsync([FromRoute] long key)
        {
            return await FindUsingEfAsync(key, _ => _.
                Include(_ => _.From).
                Include(_ => _.To).
                Include(_ => _.Train).
                Include(_ => _.Wagon).
                Include(_ => _.TrainSchedule).
                Include(_ => _.Segments));
        }

    }
}
