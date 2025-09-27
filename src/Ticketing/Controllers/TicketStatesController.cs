using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Mappings;
using Ticketing.Models.Dtos;
using Ticketing.Models.Queries.TicketStates;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;

namespace Ticketing.Controllers
{
    /// <summary>
    /// Статус билета
    /// </summary>
    [Route("/api/v1/ticketStates")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class TicketStatesController : RestControllerBase2<TicketState, long, TicketStateDto, TicketStateQuery, TicketStateMap>
    {
        public TicketStatesController(ILogger<RestServiceBase<TicketState, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            TicketStateMap ticketStateMap)
            : base(logger,
                restDapperDb,
                restDb,
                "TicketStates",
                ticketStateMap)
        {
        }

        /// <summary>
        /// Search of TicketState using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of ticketStates</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/ticketStates/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<TicketStateDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<TicketStateDto>> SearchAsync([FromBody] TicketStateQuery query)
        {
            return await base.SearchAsync(query);
        }

        /// <summary>
        /// Get the ticketState by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">TicketState data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/ticketStates/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(TicketStateDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<TicketStateDto> FindAsync([FromRoute] long key)
        {
            return await base.FindAsync(key);
        }

    }
}
