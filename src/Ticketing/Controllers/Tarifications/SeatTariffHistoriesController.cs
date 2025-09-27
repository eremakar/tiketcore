using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities.Tarifications;
using Ticketing.Mappings.Tarifications;
using Ticketing.Models.Dtos.Tarifications;
using Ticketing.Models.Queries.Tarifications.SeatTariffHistories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;

namespace Ticketing.Controllers.Tarifications
{
    /// <summary>
    /// История тарифа места в вагоне
    /// </summary>
    [Route("/api/v1/seatTariffHistories")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class SeatTariffHistoriesController : RestControllerBase2<SeatTariffHistory, long, SeatTariffHistoryDto, SeatTariffHistoryQuery, SeatTariffHistoryMap>
    {
        public SeatTariffHistoriesController(ILogger<RestServiceBase<SeatTariffHistory, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            SeatTariffHistoryMap seatTariffHistoryMap)
            : base(logger,
                restDapperDb,
                restDb,
                "SeatTariffHistories",
                seatTariffHistoryMap)
        {
        }

        /// <summary>
        /// Search of SeatTariffHistory using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of seatTariffHistories</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/seatTariffHistories/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<SeatTariffHistoryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<SeatTariffHistoryDto>> SearchAsync([FromBody] SeatTariffHistoryQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.BaseFare).
                Include(_ => _.Train).
                Include(_ => _.TrainCategory).
                Include(_ => _.WagonClass).
                Include(_ => _.Season).
                Include(_ => _.SeatType).
                Include(_ => _.Connection));
        }

        /// <summary>
        /// Get the seatTariffHistory by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">SeatTariffHistory data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/seatTariffHistories/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(SeatTariffHistoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<SeatTariffHistoryDto> FindAsync([FromRoute] long key)
        {
            return await FindUsingEfAsync(key, _ => _.
                Include(_ => _.BaseFare).
                Include(_ => _.Train).
                Include(_ => _.TrainCategory).
                Include(_ => _.WagonClass).
                Include(_ => _.Season).
                Include(_ => _.SeatType).
                Include(_ => _.Connection));
        }

    }
}
