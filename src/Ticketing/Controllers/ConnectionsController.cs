using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Mappings;
using Ticketing.Models.Dtos;
using Ticketing.Models.Queries.Connections;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;

namespace Ticketing.Controllers
{
    /// <summary>
    /// Соединение 2х станций
    /// </summary>
    [Route("/api/v1/connections")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class ConnectionsController : RestControllerBase2<Connection, long, ConnectionDto, ConnectionQuery, ConnectionMap>
    {
        public ConnectionsController(ILogger<RestServiceBase<Connection, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            ConnectionMap connectionMap)
            : base(logger,
                restDapperDb,
                restDb,
                "Connections",
                connectionMap)
        {
        }

        /// <summary>
        /// Search of Connection using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of connections</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/connections/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<ConnectionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<ConnectionDto>> SearchAsync([FromBody] ConnectionQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.From).
                Include(_ => _.To));
        }

        /// <summary>
        /// Get the connection by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">Connection data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/connections/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(ConnectionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<ConnectionDto> FindAsync([FromRoute] long key)
        {
            return await FindUsingEfAsync(key, _ => _.
                Include(_ => _.From).
                Include(_ => _.To));
        }

    }
}
