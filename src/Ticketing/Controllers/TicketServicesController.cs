using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Mappings;
using Ticketing.Models.Dtos;
using Ticketing.Models.Queries.TicketServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;

namespace Ticketing.Controllers
{
    /// <summary>
    /// Услуги билета
    /// </summary>
    [Route("/api/v1/ticketServices")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class TicketServicesController : RestControllerBase2<TicketService, long, TicketServiceDto, TicketServiceQuery, TicketServiceMap>
    {
        public TicketServicesController(ILogger<RestServiceBase<TicketService, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            TicketServiceMap ticketServiceMap)
            : base(logger,
                restDapperDb,
                restDb,
                "TicketServices",
                ticketServiceMap)
        {
        }

        /// <summary>
        /// Search of TicketService using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of ticketServices</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/ticketServices/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<TicketServiceDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<TicketServiceDto>> SearchAsync([FromBody] TicketServiceQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.Ticket).
                Include(_ => _.Service));
        }

        /// <summary>
        /// Get the ticketService by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">TicketService data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/ticketServices/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(TicketServiceDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<TicketServiceDto> FindAsync([FromRoute] long key)
        {
            return await FindUsingEfAsync(key, _ => _.
                Include(_ => _.Ticket).
                Include(_ => _.Service));
        }

    }
}
