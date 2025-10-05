using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Mappings;
using Ticketing.Models.Dtos;
using Ticketing.Models.Queries.SeatPurposes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;

namespace Ticketing.Controllers
{
    /// <summary>
    /// Назначение места
    /// </summary>
    [Route("/api/v1/seatPurposes")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class SeatPurposesController : RestControllerBase2<SeatPurpose, long, SeatPurposeDto, SeatPurposeQuery, SeatPurposeMap>
    {
        public SeatPurposesController(ILogger<RestServiceBase<SeatPurpose, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            SeatPurposeMap seatPurposeMap)
            : base(logger,
                restDapperDb,
                restDb,
                "SeatPurposes",
                seatPurposeMap)
        {
        }

        /// <summary>
        /// Search of SeatPurpose using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of seatPurposes</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/seatPurposes/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<SeatPurposeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<SeatPurposeDto>> SearchAsync([FromBody] SeatPurposeQuery query)
        {
            return await base.SearchAsync(query);
        }

        /// <summary>
        /// Get the seatPurpose by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">SeatPurpose data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/seatPurposes/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(SeatPurposeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<SeatPurposeDto> FindAsync([FromRoute] long key)
        {
            return await base.FindAsync(key);
        }

    }
}
