using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Mappings;
using Ticketing.Models.Dtos;
using Ticketing.Models.Queries.Filials;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;

namespace Ticketing.Controllers
{
    /// <summary>
    /// Филиал
    /// </summary>
    [Route("/api/v1/filials")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class FilialsController : RestControllerBase2<Filial, long, FilialDto, FilialQuery, FilialMap>
    {
        public FilialsController(ILogger<RestServiceBase<Filial, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            FilialMap filialMap)
            : base(logger,
                restDapperDb,
                restDb,
                "Filials",
                filialMap)
        {
        }

        /// <summary>
        /// Search of Filial using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of filials</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/filials/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<FilialDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<FilialDto>> SearchAsync([FromBody] FilialQuery query)
        {
            return await base.SearchAsync(query);
        }

        /// <summary>
        /// Get the filial by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">Filial data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/filials/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(FilialDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<FilialDto> FindAsync([FromRoute] long key)
        {
            return await base.FindAsync(key);
        }

    }
}
