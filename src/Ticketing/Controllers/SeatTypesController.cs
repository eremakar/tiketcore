using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Mappings;
using Ticketing.Models.Dtos;
using Ticketing.Models.Queries.SeatTypes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;

namespace Ticketing.Controllers
{
    /// <summary>
    /// Тип места
    /// </summary>
    [Route("/api/v1/seatTypes")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class SeatTypesController : RestControllerBase2<SeatType, long, SeatTypeDto, SeatTypeQuery, SeatTypeMap>
    {
        public SeatTypesController(ILogger<RestServiceBase<SeatType, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            SeatTypeMap seatTypeMap)
            : base(logger,
                restDapperDb,
                restDb,
                "SeatTypes",
                seatTypeMap)
        {
        }

        /// <summary>
        /// Search of SeatType using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of seatTypes</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/seatTypes/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<SeatTypeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<SeatTypeDto>> SearchAsync([FromBody] SeatTypeQuery query)
        {
            return await base.SearchAsync(query);
        }

        /// <summary>
        /// Get the seatType by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">SeatType data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/seatTypes/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(SeatTypeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<SeatTypeDto> FindAsync([FromRoute] long key)
        {
            return await base.FindAsync(key);
        }

    }
}
