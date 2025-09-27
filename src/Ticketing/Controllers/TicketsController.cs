using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Mappings;
using Ticketing.Models.Dtos;
using Ticketing.Models.Queries.Tickets;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;

namespace Ticketing.Controllers
{
    /// <summary>
    /// Билет
    /// </summary>
    [Route("/api/v1/tickets")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class TicketsController : RestControllerBase2<Ticket, long, TicketDto, TicketQuery, TicketMap>
    {
        public TicketsController(ILogger<RestServiceBase<Ticket, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            TicketMap ticketMap)
            : base(logger,
                restDapperDb,
                restDb,
                "Tickets",
                ticketMap)
        {
        }

        /// <summary>
        /// Search of Ticket using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of tickets</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/tickets/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<TicketDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<TicketDto>> SearchAsync([FromBody] TicketQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.From).
                Include(_ => _.To).
                Include(_ => _.Train).
                Include(_ => _.Wagon).
                Include(_ => _.Seat).
                Include(_ => _.TrainSchedule));
        }

        /// <summary>
        /// Get the ticket by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">Ticket data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/tickets/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(TicketDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<TicketDto> FindAsync([FromRoute] long key)
        {
            return await FindUsingEfAsync(key, _ => _.
                Include(_ => _.From).
                Include(_ => _.To).
                Include(_ => _.Train).
                Include(_ => _.Wagon).
                Include(_ => _.Seat).
                Include(_ => _.TrainSchedule));
        }

    }
}
