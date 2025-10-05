using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities.Tarifications;
using Ticketing.Mappings.Tarifications;
using Ticketing.Models.Dtos.Tarifications;
using Ticketing.Models.Queries.Tarifications.TariffTrainCategoryItems;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;

namespace Ticketing.Controllers.Tarifications
{
    /// <summary>
    /// Элемент тарифа категории поезда
    /// </summary>
    [Route("/api/v1/tariffTrainCategoryItems")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class TariffTrainCategoryItemsController : RestControllerBase2<TariffTrainCategoryItem, long, TariffTrainCategoryItemDto, TariffTrainCategoryItemQuery, TariffTrainCategoryItemMap>
    {
        public TariffTrainCategoryItemsController(ILogger<RestServiceBase<TariffTrainCategoryItem, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            TariffTrainCategoryItemMap tariffTrainCategoryItemMap)
            : base(logger,
                restDapperDb,
                restDb,
                "TariffTrainCategoryItems",
                tariffTrainCategoryItemMap)
        {
        }

        /// <summary>
        /// Search of TariffTrainCategoryItem using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of tariffTrainCategoryItems</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/tariffTrainCategoryItems/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<TariffTrainCategoryItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<TariffTrainCategoryItemDto>> SearchAsync([FromBody] TariffTrainCategoryItemQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.TrainCategory).
                Include(_ => _.Tariff));
        }

        /// <summary>
        /// Get the tariffTrainCategoryItem by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">TariffTrainCategoryItem data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/tariffTrainCategoryItems/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(TariffTrainCategoryItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<TariffTrainCategoryItemDto> FindAsync([FromRoute] long key)
        {
            return await FindUsingEfAsync(key, _ => _.
                Include(_ => _.TrainCategory).
                Include(_ => _.Tariff));
        }

    }
}
