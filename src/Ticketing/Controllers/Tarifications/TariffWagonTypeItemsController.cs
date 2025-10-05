using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities.Tarifications;
using Ticketing.Mappings.Tarifications;
using Ticketing.Models.Dtos.Tarifications;
using Ticketing.Models.Queries.Tarifications.TariffWagonTypeItems;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;

namespace Ticketing.Controllers.Tarifications
{
    /// <summary>
    /// Элемент тарифа типа вагона
    /// </summary>
    [Route("/api/v1/tariffWagonTypeItems")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class TariffWagonTypeItemsController : RestControllerBase2<TariffWagonTypeItem, long, TariffWagonTypeItemDto, TariffWagonTypeItemQuery, TariffWagonTypeItemMap>
    {
        public TariffWagonTypeItemsController(ILogger<RestServiceBase<TariffWagonTypeItem, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            TariffWagonTypeItemMap tariffWagonTypeItemMap)
            : base(logger,
                restDapperDb,
                restDb,
                "TariffWagonTypeItems",
                tariffWagonTypeItemMap)
        {
        }

        /// <summary>
        /// Search of TariffWagonTypeItem using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of tariffWagonTypeItems</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/tariffWagonTypeItems/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<TariffWagonTypeItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<TariffWagonTypeItemDto>> SearchAsync([FromBody] TariffWagonTypeItemQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.WagonType).
                Include(_ => _.Tariff));
        }

        /// <summary>
        /// Get the tariffWagonTypeItem by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">TariffWagonTypeItem data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/tariffWagonTypeItems/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(TariffWagonTypeItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<TariffWagonTypeItemDto> FindAsync([FromRoute] long key)
        {
            return await FindUsingEfAsync(key, _ => _.
                Include(_ => _.WagonType).
                Include(_ => _.Tariff));
        }

    }
}
