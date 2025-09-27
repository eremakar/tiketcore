using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities.Tarifications;
using Ticketing.Mappings.Tarifications;
using Ticketing.Models.Dtos.Tarifications;
using Ticketing.Models.Queries.Tarifications.WagonClasses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;

namespace Ticketing.Controllers.Tarifications
{
    /// <summary>
    /// Класс вагона
    /// </summary>
    [Route("/api/v1/wagonClasses")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class WagonClassesController : RestControllerBase2<WagonClass, long, WagonClassDto, WagonClassQuery, WagonClassMap>
    {
        public WagonClassesController(ILogger<RestServiceBase<WagonClass, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            WagonClassMap wagonClassMap)
            : base(logger,
                restDapperDb,
                restDb,
                "WagonClasses",
                wagonClassMap)
        {
        }

        /// <summary>
        /// Search of WagonClass using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of wagonClasses</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/wagonClasses/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<WagonClassDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<WagonClassDto>> SearchAsync([FromBody] WagonClassQuery query)
        {
            return await base.SearchAsync(query);
        }

        /// <summary>
        /// Get the wagonClass by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">WagonClass data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/wagonClasses/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(WagonClassDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<WagonClassDto> FindAsync([FromRoute] long key)
        {
            return await base.FindAsync(key);
        }

    }
}
