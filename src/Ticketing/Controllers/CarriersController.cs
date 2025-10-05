using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Mappings;
using Ticketing.Models.Dtos;
using Ticketing.Models.Queries.Carriers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;

namespace Ticketing.Controllers
{
    /// <summary>
    /// Перевозчик
    /// </summary>
    [Route("/api/v1/carriers")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class CarriersController : RestControllerBase2<Carrier, long, CarrierDto, CarrierQuery, CarrierMap>
    {
        public CarriersController(ILogger<RestServiceBase<Carrier, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            CarrierMap carrierMap)
            : base(logger,
                restDapperDb,
                restDb,
                "Carriers",
                carrierMap)
        {
        }

        /// <summary>
        /// Search of Carrier using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of carriers</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/carriers/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<CarrierDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<CarrierDto>> SearchAsync([FromBody] CarrierQuery query)
        {
            return await base.SearchAsync(query);
        }

        /// <summary>
        /// Get the carrier by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">Carrier data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/carriers/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(CarrierDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<CarrierDto> FindAsync([FromRoute] long key)
        {
            return await base.FindAsync(key);
        }

    }
}
