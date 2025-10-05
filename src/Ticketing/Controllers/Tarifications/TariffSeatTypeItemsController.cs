using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities.Tarifications;
using Ticketing.Mappings.Tarifications;
using Ticketing.Models.Dtos.Tarifications;
using Ticketing.Models.Queries.Tarifications.TariffSeatTypeItems;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;

namespace Ticketing.Controllers.Tarifications
{
    /// <summary>
    /// Элемент тарифа типа места
    /// </summary>
    [Route("/api/v1/tariffSeatTypeItems")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class TariffSeatTypeItemsController : RestControllerBase2<TariffSeatTypeItem, long, TariffSeatTypeItemDto, TariffSeatTypeItemQuery, TariffSeatTypeItemMap>
    {
        public TariffSeatTypeItemsController(ILogger<RestServiceBase<TariffSeatTypeItem, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            TariffSeatTypeItemMap tariffSeatTypeItemMap)
            : base(logger,
                restDapperDb,
                restDb,
                "TariffSeatTypeItems",
                tariffSeatTypeItemMap)
        {
        }

        /// <summary>
        /// Search of TariffSeatTypeItem using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of tariffSeatTypeItems</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/tariffSeatTypeItems/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<TariffSeatTypeItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<TariffSeatTypeItemDto>> SearchAsync([FromBody] TariffSeatTypeItemQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.SeatType).
                Include(_ => _.TariffWagon).ThenInclude(_ => _.Wagon));
        }

        /// <summary>
        /// Get the tariffSeatTypeItem by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">TariffSeatTypeItem data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/tariffSeatTypeItems/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(TariffSeatTypeItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<TariffSeatTypeItemDto> FindAsync([FromRoute] long key)
        {
            return await FindUsingEfAsync(key, _ => _.
                Include(_ => _.SeatType).
                Include(_ => _.TariffWagon).ThenInclude(_ => _.Wagon));
        }

    }
}
