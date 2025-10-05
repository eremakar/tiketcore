using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities.Tarifications;
using Ticketing.Mappings.Tarifications;
using Ticketing.Models.Dtos.Tarifications;
using Ticketing.Models.Queries.Tarifications.TariffWagonItems;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;

namespace Ticketing.Controllers.Tarifications
{
    /// <summary>
    /// Элемент тарифа вагона
    /// </summary>
    [Route("/api/v1/tariffWagonItems")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class TariffWagonItemsController : RestControllerBase2<TariffWagonItem, long, TariffWagonItemDto, TariffWagonItemQuery, TariffWagonItemMap>
    {
        public TariffWagonItemsController(ILogger<RestServiceBase<TariffWagonItem, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            TariffWagonItemMap tariffWagonItemMap)
            : base(logger,
                restDapperDb,
                restDb,
                "TariffWagonItems",
                tariffWagonItemMap)
        {
        }

        /// <summary>
        /// Search of TariffWagonItem using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of tariffWagonItems</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/tariffWagonItems/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<TariffWagonItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<TariffWagonItemDto>> SearchAsync([FromBody] TariffWagonItemQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.Wagon).
                Include(_ => _.SeatTypes).
                Include(_ => _.Tariff));
        }

        /// <summary>
        /// Get the tariffWagonItem by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">TariffWagonItem data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/tariffWagonItems/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(TariffWagonItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<TariffWagonItemDto> FindAsync([FromRoute] long key)
        {
            return await FindUsingEfAsync(key, _ => _.
                Include(_ => _.Wagon).
                Include(_ => _.SeatTypes).
                Include(_ => _.Tariff));
        }

    }
}
