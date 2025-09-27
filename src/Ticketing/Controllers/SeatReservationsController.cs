using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Mappings;
using Ticketing.Models.Dtos;
using Ticketing.Models.Queries.SeatReservations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;
using Ticketing.Services;

namespace Ticketing.Controllers
{
    /// <summary>
    /// Бронирование места
    /// </summary>
    [Route("/api/v1/seatReservations")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class SeatReservationsController : RestControllerBase2<SeatReservation, long, SeatReservationDto, SeatReservationQuery, SeatReservationMap>
    {
        private readonly SeatReservationsService service;

        /// <summary>
        /// Controller ctor
        /// </summary>
        public SeatReservationsController(ILogger<RestServiceBase<SeatReservation, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            SeatReservationMap seatReservationMap,
            SeatReservationsService service)
            : base(logger,
                restDapperDb,
                restDb,
                "SeatReservations",
                seatReservationMap)
        {
            this.service = service;
        }

        /// <summary>
        /// Search of SeatReservation using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of seatReservations</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/seatReservations/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<SeatReservationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<SeatReservationDto>> SearchAsync([FromBody] SeatReservationQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.From).
                Include(_ => _.To).
                Include(_ => _.Train).
                Include(_ => _.Wagon).
                Include(_ => _.Seat).
                Include(_ => _.TrainSchedule).
                Include(_ => _.Segments));
        }

        /// <summary>
        /// Get the seatReservation by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">SeatReservation data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/seatReservations/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(SeatReservationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<SeatReservationDto> FindAsync([FromRoute] long key)
        {
            return await FindUsingEfAsync(key, _ => _.
                Include(_ => _.From).
                Include(_ => _.To).
                Include(_ => _.Train).
                Include(_ => _.Wagon).
                Include(_ => _.Seat).
                Include(_ => _.TrainSchedule).
                Include(_ => _.Segments));
        }

    }
}
