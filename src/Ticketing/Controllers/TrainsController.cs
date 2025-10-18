using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Mappings;
using Ticketing.Models.Dtos;
using Ticketing.Models.Queries.Trains;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;

namespace Ticketing.Controllers
{
    /// <summary>
    /// Поезд по маршруту
    /// </summary>
    [Route("/api/v1/trains")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class TrainsController : RestControllerBase2<Train, long, TrainDto, TrainQuery, TrainMap>
    {
        public TrainsController(ILogger<RestServiceBase<Train, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            TrainMap trainMap)
            : base(logger,
                restDapperDb,
                restDb,
                "Trains",
                trainMap)
        {
        }

        /// <summary>
        /// Search of Train using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of trains</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/trains/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<TrainDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<TrainDto>> SearchAsync([FromBody] TrainQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.Type).
                Include(_ => _.From).
                Include(_ => _.To).
                Include(_ => _.Route).
                Include(_ => _.Periodicity).
                Include(_ => _.Plan).
                Include(_ => _.Category).
                Include(_ => _.Tariff));
        }

        /// <summary>
        /// Get the train by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">Train data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/trains/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(TrainDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<TrainDto> FindAsync([FromRoute] long key)
        {
            return await FindUsingEfAsync(key, _ => _.
                Include(_ => _.Type).
                Include(_ => _.From).
                Include(_ => _.To).
                Include(_ => _.Route).
                Include(_ => _.Periodicity).
                Include(_ => _.Plan).
                Include(_ => _.Category).
                Include(_ => _.Tariff));
        }

    }
}
