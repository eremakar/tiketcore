using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities.Tarifications;
using Ticketing.Mappings.Tarifications;
using Ticketing.Models.Dtos.Tarifications;
using Ticketing.Models.Queries.Tarifications.TariffSeatTypeItems;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;

namespace Ticketing.Controllers.Tarifications
{
    public partial class TariffSeatTypeItemsController
    {
        /// <summary>
        /// Add new tariffSeatTypeItem
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">Unique registered id</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/tariffSeatTypeItems")]
        [HttpPost]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<object> AddAsync([FromBody] TariffSeatTypeItemDto request)
        {
            return await base.AddAsync(request);
        }

        /// <summary>
        /// Update tariffSeatTypeItem
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">OK</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/tariffSeatTypeItems")]
        [HttpPut]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<object> UpdateAsync([FromBody] TariffSeatTypeItemDto request)
        {
            return await base.UpdateAsync(request);
        }

        /// <summary>
        /// Patch tariffSeatTypeItem
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">OK</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/tariffSeatTypeItems/patch")]
        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<IActionResult> PatchAsync(long id, [FromBody] JsonPatchDocument<TariffSeatTypeItemDto> patch)
        {
            return await base.PatchAsync(id, patch);
        }

        /// <summary>
        /// Remove tariffSeatTypeItem
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">OK</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/tariffSeatTypeItems/{key}")]
        [HttpDelete]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<object> RemoveAsync([FromRoute] long key)
        {
            return await base.RemoveAsync(key);
        }

    }
}
