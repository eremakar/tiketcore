using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Tarifications.Data.TicketDb.DatabaseContext;
using Ticketing.Tarifications.Data.TicketDb.Entities.Tarifications;
using Ticketing.Tarifications.Mappings.Tarifications;
using Ticketing.Tarifications.Models.Dtos.Tarifications;
using Ticketing.Tarifications.Models.Queries.Tarifications.Tariffs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;

namespace Ticketing.Tarifications.Controllers.Tarifications
{
    /// <summary>
    /// Тариф
    /// </summary>
    [Route("/api/v1/tariffs")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class TariffsController : RestControllerBase2<Tariff, long, TariffDto, TariffQuery, TariffMap>
    {
        public TariffsController(ILogger<RestServiceBase<Tariff, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            TariffMap tariffMap)
            : base(logger,
                restDapperDb,
                restDb,
                "Tariffs",
                tariffMap)
        {
        }

        /// <summary>
        /// Search of Tariff using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of tariffs</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/tariffs/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<TariffDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<TariffDto>> SearchAsync([FromBody] TariffQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.BaseFare).
                Include(_ => _.TrainCategory).
                Include(_ => _.Wagon));
        }

        /// <summary>
        /// Get the tariff by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">Tariff data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/tariffs/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(TariffDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<TariffDto> FindAsync([FromRoute] long key)
        {
            return await FindUsingEfAsync(key, _ => _.
                Include(_ => _.BaseFare).
                Include(_ => _.TrainCategory).
                Include(_ => _.Wagon));
        }

    }
}
