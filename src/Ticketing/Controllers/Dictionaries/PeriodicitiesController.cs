using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities.Dictionaries;
using Ticketing.Mappings.Dictionaries;
using Ticketing.Models.Dtos.Dictionaries;
using Ticketing.Models.Queries.Dictionaries.Periodicities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;

namespace Ticketing.Controllers.Dictionaries
{
    /// <summary>
    /// Периодичность
    /// </summary>
    [Route("/api/v1/periodicities")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class PeriodicitiesController : RestControllerBase2<Periodicity, long, PeriodicityDto, PeriodicityQuery, PeriodicityMap>
    {
        public PeriodicitiesController(ILogger<RestServiceBase<Periodicity, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            PeriodicityMap periodicityMap)
            : base(logger,
                restDapperDb,
                restDb,
                "Periodicities",
                periodicityMap)
        {
        }

        /// <summary>
        /// Search of Periodicity using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of periodicities</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/periodicities/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<PeriodicityDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<PeriodicityDto>> SearchAsync([FromBody] PeriodicityQuery query)
        {
            return await base.SearchAsync(query);
        }

        /// <summary>
        /// Get the periodicity by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">Periodicity data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/periodicities/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(PeriodicityDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PeriodicityDto> FindAsync([FromRoute] long key)
        {
            return await base.FindAsync(key);
        }

    }
}
