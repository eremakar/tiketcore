using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Mappings;
using Ticketing.Models.Dtos;
using Ticketing.Models.Queries.WagonModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;
using Ticketing.Services;

namespace Ticketing.Controllers
{
    /// <summary>
    /// Вагон (тип)
    /// </summary>
    [Route("/api/v1/wagonModels")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class WagonModelsController : RestControllerBase2<WagonModel, long, WagonModelDto, WagonModelQuery, WagonModelMap>
    {
        protected TicketDbContext db;
        private readonly WagonModelsService wagonModelsService;
        private readonly WagonModelFeatureMap wagonModelFeatureMap;
        private readonly SeatMap seatMap;

        public WagonModelsController(ILogger<RestServiceBase<WagonModel, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            WagonModelMap wagonModelMap,
            WagonModelsService wagonModelsService,
            WagonModelFeatureMap wagonModelFeatureMap,
            SeatMap seatMap)
            : base(logger,
                restDapperDb,
                restDb,
                "WagonModels",
                wagonModelMap)
        {
            db = restDb;
            this.wagonModelsService = wagonModelsService;
            this.wagonModelFeatureMap = wagonModelFeatureMap;
            this.seatMap = seatMap;
        }

        /// <summary>
        /// Search of WagonModel using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of wagonModels</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/wagonModels/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<WagonModelDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<WagonModelDto>> SearchAsync([FromBody] WagonModelQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.Class).
                Include(_ => _.Type).
                Include(_ => _.Features).
                Include(_ => _.Seats));
        }

        /// <summary>
        /// Get the wagonModel by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">WagonModel data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/wagonModels/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(WagonModelDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<WagonModelDto> FindAsync([FromRoute] long key)
        {
            return await FindUsingEfAsync(key, _ => _.
                Include(_ => _.Class).
                Include(_ => _.Type).
                Include(_ => _.Features).
                Include(_ => _.Seats));
        }

    }
}
