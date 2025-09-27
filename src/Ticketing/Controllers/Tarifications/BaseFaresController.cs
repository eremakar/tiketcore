using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities.Tarifications;
using Ticketing.Mappings.Tarifications;
using Ticketing.Models.Dtos.Tarifications;
using Ticketing.Models.Queries.Tarifications.BaseFares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;

namespace Ticketing.Controllers.Tarifications
{
    /// <summary>
    /// Базовая ставка
    /// </summary>
    [Route("/api/v1/baseFares")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class BaseFaresController : RestControllerBase2<BaseFare, long, BaseFareDto, BaseFareQuery, BaseFareMap>
    {
        public BaseFaresController(ILogger<RestServiceBase<BaseFare, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            BaseFareMap baseFareMap)
            : base(logger,
                restDapperDb,
                restDb,
                "BaseFares",
                baseFareMap)
        {
        }

        /// <summary>
        /// Search of BaseFare using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of baseFares</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/baseFares/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<BaseFareDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<BaseFareDto>> SearchAsync([FromBody] BaseFareQuery query)
        {
            return await base.SearchAsync(query);
        }

        /// <summary>
        /// Get the baseFare by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">BaseFare data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/baseFares/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(BaseFareDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<BaseFareDto> FindAsync([FromRoute] long key)
        {
            return await base.FindAsync(key);
        }

    }
}
