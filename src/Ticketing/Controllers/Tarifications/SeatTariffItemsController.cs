using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities.Tarifications;
using Ticketing.Mappings.Tarifications;
using Ticketing.Models.Dtos.Tarifications;
using Ticketing.Models.Queries.Tarifications.SeatTariffItems;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;

namespace Ticketing.Controllers.Tarifications
{
    /// <summary>
    /// Элемент тарифа места
    /// </summary>
    [Route("/api/v1/seatTariffItems")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class SeatTariffItemsController : RestControllerBase2<SeatTariffItem, long, SeatTariffItemDto, SeatTariffItemQuery, SeatTariffItemMap>
    {
        public SeatTariffItemsController(ILogger<RestServiceBase<SeatTariffItem, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            SeatTariffItemMap seatTariffItemMap)
            : base(logger,
                restDapperDb,
                restDb,
                "SeatTariffItems",
                seatTariffItemMap)
        {
        }

        /// <summary>
        /// Search of SeatTariffItem using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of seatTariffItems</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/seatTariffItems/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<SeatTariffItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<SeatTariffItemDto>> SearchAsync([FromBody] SeatTariffItemQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.WagonClass).
                Include(_ => _.Season).
                Include(_ => _.SeatType).
                Include(_ => _.From).
                Include(_ => _.To).
                Include(_ => _.SeatTariff));
        }

        /// <summary>
        /// Get the seatTariffItem by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">SeatTariffItem data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/seatTariffItems/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(SeatTariffItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<SeatTariffItemDto> FindAsync([FromRoute] long key)
        {
            return await FindUsingEfAsync(key, _ => _.
                Include(_ => _.WagonClass).
                Include(_ => _.Season).
                Include(_ => _.SeatType).
                Include(_ => _.From).
                Include(_ => _.To).
                Include(_ => _.SeatTariff));
        }

    }
}
