using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Mappings;
using Ticketing.Models.Dtos;
using Ticketing.Models.Queries.WagonModelFeatures;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;

namespace Ticketing.Controllers
{
    public partial class WagonModelFeaturesController
    {
        /// <summary>
        /// Add new wagonModelFeature
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">Unique registered id</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/wagonModelFeatures")]
        [HttpPost]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<object> AddAsync([FromBody] WagonModelFeatureDto request)
        {
            return await base.AddAsync(request);
        }

        /// <summary>
        /// Update wagonModelFeature
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">OK</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/wagonModelFeatures")]
        [HttpPut]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<object> UpdateAsync([FromBody] WagonModelFeatureDto request)
        {
            return await base.UpdateAsync(request);
        }

        /// <summary>
        /// Patch wagonModelFeature
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">OK</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/wagonModelFeatures/patch")]
        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<IActionResult> PatchAsync(long id, [FromBody] JsonPatchDocument<WagonModelFeatureDto> patch)
        {
            return await base.PatchAsync(id, patch);
        }

        /// <summary>
        /// Remove wagonModelFeature
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">OK</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/wagonModelFeatures/{key}")]
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
