using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Mappings;
using Ticketing.Models.Dtos;
using Ticketing.Models.Queries.WagonFeatures;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;

namespace Ticketing.Controllers
{
    /// <summary>
    /// Особенности вагона
    /// </summary>
    [Route("/api/v1/wagonFeatures")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class WagonFeaturesController : RestControllerBase2<WagonFeature, int, WagonFeatureDto, WagonFeatureQuery, WagonFeatureMap>
    {
        public WagonFeaturesController(ILogger<RestServiceBase<WagonFeature, int>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            WagonFeatureMap wagonFeatureMap)
            : base(logger,
                restDapperDb,
                restDb,
                "WagonFeatures",
                wagonFeatureMap)
        {
        }

        /// <summary>
        /// Search of WagonFeature using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of wagonFeatures</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/wagonFeatures/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<WagonFeatureDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<WagonFeatureDto>> SearchAsync([FromBody] WagonFeatureQuery query)
        {
            return await base.SearchAsync(query);
        }

        /// <summary>
        /// Get the wagonFeature by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">WagonFeature data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/wagonFeatures/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(WagonFeatureDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<WagonFeatureDto> FindAsync([FromRoute] int key)
        {
            return await base.FindAsync(key);
        }

    }
}
