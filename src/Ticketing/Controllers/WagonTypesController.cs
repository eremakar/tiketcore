using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Mappings;
using Ticketing.Models.Dtos;
using Ticketing.Models.Queries.WagonTypes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;

namespace Ticketing.Controllers
{
    /// <summary>
    /// Тип вагона
    /// </summary>
    [Route("/api/v1/wagonTypes")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class WagonTypesController : RestControllerBase2<WagonType, long, WagonTypeDto, WagonTypeQuery, WagonTypeMap>
    {
        public WagonTypesController(ILogger<RestServiceBase<WagonType, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            WagonTypeMap wagonTypeMap)
            : base(logger,
                restDapperDb,
                restDb,
                "WagonTypes",
                wagonTypeMap)
        {
        }

        /// <summary>
        /// Search of WagonType using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of wagonTypes</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/wagonTypes/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<WagonTypeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<WagonTypeDto>> SearchAsync([FromBody] WagonTypeQuery query)
        {
            return await base.SearchAsync(query);
        }

        /// <summary>
        /// Get the wagonType by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">WagonType data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/wagonTypes/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(WagonTypeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<WagonTypeDto> FindAsync([FromRoute] long key)
        {
            return await base.FindAsync(key);
        }

    }
}
