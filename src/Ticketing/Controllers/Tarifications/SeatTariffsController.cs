using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities.Tarifications;
using Ticketing.Mappings.Tarifications;
using Ticketing.Models.Dtos.Tarifications;
using Ticketing.Models.Queries.Tarifications.SeatTariffs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;

namespace Ticketing.Controllers.Tarifications
{
    /// <summary>
    /// Тариф места в вагоне
    /// </summary>
    [Route("/api/v1/seatTariffs")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class SeatTariffsController : RestControllerBase2<SeatTariff, long, SeatTariffDto, SeatTariffQuery, SeatTariffMap>
    {
        public SeatTariffsController(ILogger<RestServiceBase<SeatTariff, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            SeatTariffMap seatTariffMap)
            : base(logger,
                restDapperDb,
                restDb,
                "SeatTariffs",
                seatTariffMap)
        {
        }

        /// <summary>
        /// Search of SeatTariff using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of seatTariffs</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/seatTariffs/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<SeatTariffDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<SeatTariffDto>> SearchAsync([FromBody] SeatTariffQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.Train).
                Include(_ => _.BaseFare).
                Include(_ => _.TrainCategory).
                Include(_ => _.Items).
                Include(_ => _.Tariff));
        }

        /// <summary>
        /// Get the seatTariff by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">SeatTariff data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/seatTariffs/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(SeatTariffDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<SeatTariffDto> FindAsync([FromRoute] long key)
        {
            return await FindUsingEfAsync(key, _ => _.
                Include(_ => _.Train).
                Include(_ => _.BaseFare).
                Include(_ => _.TrainCategory).
                Include(_ => _.Items).
                Include(_ => _.Tariff));
        }

    }
}
