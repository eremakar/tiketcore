using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities.Tarifications;
using Ticketing.Mappings.Tarifications;
using Ticketing.Models.Dtos.Tarifications;
using Ticketing.Models.Queries.Tarifications.TrainCategories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;

namespace Ticketing.Controllers.Tarifications
{
    /// <summary>
    /// Категория поезда
    /// </summary>
    [Route("/api/v1/trainCategories")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class TrainCategoriesController : RestControllerBase2<TrainCategory, long, TrainCategoryDto, TrainCategoryQuery, TrainCategoryMap>
    {
        public TrainCategoriesController(ILogger<RestServiceBase<TrainCategory, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            TrainCategoryMap trainCategoryMap)
            : base(logger,
                restDapperDb,
                restDb,
                "TrainCategories",
                trainCategoryMap)
        {
        }

        /// <summary>
        /// Search of TrainCategory using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of trainCategories</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/trainCategories/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<TrainCategoryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<TrainCategoryDto>> SearchAsync([FromBody] TrainCategoryQuery query)
        {
            return await base.SearchAsync(query);
        }

        /// <summary>
        /// Get the trainCategory by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">TrainCategory data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/trainCategories/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(TrainCategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<TrainCategoryDto> FindAsync([FromRoute] long key)
        {
            return await base.FindAsync(key);
        }

    }
}
