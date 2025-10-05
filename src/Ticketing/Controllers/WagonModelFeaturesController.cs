using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Mappings;
using Ticketing.Models.Dtos;
using Ticketing.Models.Queries.WagonModelFeatures;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;

namespace Ticketing.Controllers
{
    /// <summary>
    /// Особенности модели вагона
    /// </summary>
    [Route("/api/v1/wagonModelFeatures")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class WagonModelFeaturesController : RestControllerBase2<WagonModelFeature, long, WagonModelFeatureDto, WagonModelFeatureQuery, WagonModelFeatureMap>
    {
        public WagonModelFeaturesController(ILogger<RestServiceBase<WagonModelFeature, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            WagonModelFeatureMap wagonModelFeatureMap)
            : base(logger,
                restDapperDb,
                restDb,
                "WagonModelFeatures",
                wagonModelFeatureMap)
        {
        }

        /// <summary>
        /// Search of WagonModelFeature using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of wagonModelFeatures</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/wagonModelFeatures/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<WagonModelFeatureDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<WagonModelFeatureDto>> SearchAsync([FromBody] WagonModelFeatureQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.Wagon).
                Include(_ => _.Feature));
        }

        /// <summary>
        /// Get the wagonModelFeature by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">WagonModelFeature data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/wagonModelFeatures/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(WagonModelFeatureDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<WagonModelFeatureDto> FindAsync([FromRoute] long key)
        {
            return await FindUsingEfAsync(key, _ => _.
                Include(_ => _.Wagon).
                Include(_ => _.Feature));
        }

    }
}
