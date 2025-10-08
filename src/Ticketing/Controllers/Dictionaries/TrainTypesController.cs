using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities.Dictionaries;
using Ticketing.Mappings.Dictionaries;
using Ticketing.Models.Dtos.Dictionaries;
using Ticketing.Models.Queries.Dictionaries.TrainTypes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;

namespace Ticketing.Controllers.Dictionaries
{
    /// <summary>
    /// Тип поезда
    /// </summary>
    [Route("/api/v1/trainTypes")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class TrainTypesController : RestControllerBase2<TrainType, long, TrainTypeDto, TrainTypeQuery, TrainTypeMap>
    {
        public TrainTypesController(ILogger<RestServiceBase<TrainType, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            TrainTypeMap trainTypeMap)
            : base(logger,
                restDapperDb,
                restDb,
                "TrainTypes",
                trainTypeMap)
        {
        }

        /// <summary>
        /// Search of TrainType using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of trainTypes</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/trainTypes/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<TrainTypeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<TrainTypeDto>> SearchAsync([FromBody] TrainTypeQuery query)
        {
            return await base.SearchAsync(query);
        }

        /// <summary>
        /// Get the trainType by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">TrainType data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/trainTypes/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(TrainTypeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<TrainTypeDto> FindAsync([FromRoute] long key)
        {
            return await base.FindAsync(key);
        }

    }
}
