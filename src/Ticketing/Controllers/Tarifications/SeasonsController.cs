using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities.Tarifications;
using Ticketing.Mappings.Tarifications;
using Ticketing.Models.Dtos.Tarifications;
using Ticketing.Models.Queries.Tarifications.Seasons;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;

namespace Ticketing.Controllers.Tarifications
{
    /// <summary>
    /// Сезонность
    /// </summary>
    [Route("/api/v1/seasons")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class SeasonsController : RestControllerBase2<Season, long, SeasonDto, SeasonQuery, SeasonMap>
    {
        public SeasonsController(ILogger<RestServiceBase<Season, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            SeasonMap seasonMap)
            : base(logger,
                restDapperDb,
                restDb,
                "Seasons",
                seasonMap)
        {
        }

        /// <summary>
        /// Search of Season using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of seasons</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/seasons/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<SeasonDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<SeasonDto>> SearchAsync([FromBody] SeasonQuery query)
        {
            return await base.SearchAsync(query);
        }

        /// <summary>
        /// Get the season by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">Season data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/seasons/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(SeasonDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<SeasonDto> FindAsync([FromRoute] long key)
        {
            return await base.FindAsync(key);
        }

    }
}
