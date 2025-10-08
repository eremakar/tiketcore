using Data.Repository;
using Data.Repository.Dapper;
using Data.Repository.Helpers;
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
    public partial class WagonModelsController
    {
        /// <summary>
        /// Add new wagonModel
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">Unique registered id</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/wagonModels")]
        [HttpPost]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<object> AddAsync([FromBody] WagonModelDto request)
        {
            return await base.AddAsync(request);
        }

        /// <summary>
        /// Update wagonModel
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">OK</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/wagonModels")]
        [HttpPut]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<object> UpdateAsync([FromBody] WagonModelDto request)
        {
            // Load the original entity with Features and Seats
            var original = await db.WagonModels
                .Include(wm => wm.Features)
                .Include(wm => wm.Seats)
                .FirstOrDefaultAsync(wm => wm.Id == request.Id);

            if (original == null)
                return false;

            // Sync Features collection
            await ListHelper.SyncCollectionAsync(
                original.Features,
                request.Features,
                db.WagonModelFeatures,
                obj => obj is WagonModelFeature entity ? entity.Id : ((WagonModelFeatureDto)obj).Id,
                dto =>
                {
                    var newFeature = wagonModelFeatureMap.ReverseMap(dto);
                    newFeature.WagonId = request.Id;
                    return newFeature;
                },
                (dto, existing) =>
                {
                    var sourceFeature = wagonModelFeatureMap.ReverseMap(dto);
                    wagonModelFeatureMap.Map(sourceFeature, existing);
                });

            // Sync Seats collection
            await ListHelper.SyncCollectionAsync(
                original.Seats,
                request.Seats,
                db.Seats,
                obj => obj is Seat entity ? entity.Id : ((SeatDto)obj).Id,
                dto =>
                {
                    var newSeat = seatMap.ReverseMap(dto);
                    newSeat.WagonId = request.Id;
                    return newSeat;
                },
                (dto, existing) =>
                {
                    var sourceSeat = seatMap.ReverseMap(dto);
                    seatMap.Map(sourceSeat, existing);
                });

            // Update the main entity using base logic
            return await base.UpdateAsync(request);
        }

        /// <summary>
        /// Patch wagonModel
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">OK</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/wagonModels/patch")]
        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<IActionResult> PatchAsync(long id, [FromBody] JsonPatchDocument<WagonModelDto> patch)
        {
            return await base.PatchAsync(id, patch);
        }

        /// <summary>
        /// Remove wagonModel
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">OK</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/wagonModels/{key}")]
        [HttpDelete]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<object> RemoveAsync([FromRoute] long key)
        {
            return await base.RemoveAsync(key);
        }

        /// <summary>
        /// Generate seats for wagon model based on wagon model seat count
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">Seats generated successfully</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        /// <response code="404">Wagon model not found</response>
        [Route("/api/v1/wagonModels/{key}/seats/generate")]
        [HttpPost]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<object> GenerateSeatsAsync([FromRoute] long key)
        {
            try
            {
                var generatedSeatsCount = await wagonModelsService.GenerateSeatsAsync(key);
                
                // Get additional info for response
                var wagonModel = await db.WagonModels
                    .FirstOrDefaultAsync(wm => wm.Id == key);
                
                var existingSeats = await db.Seats
                    .Where(s => s.WagonId == key)
                    .ToListAsync();

                return new
                {
                    message = "Seats generated successfully",
                    wagonModelId = key,
                    requestedSeatCount = wagonModel?.SeatCount ?? 0,
                    existingSeatCount = existingSeats.Count - generatedSeatsCount,
                    newSeatsGenerated = generatedSeatsCount,
                    totalSeatsNow = existingSeats.Count
                };
            }
            catch (ArgumentException ex)
            {
                if (ex.Message.Contains("not found"))
                {
                    return NotFound(ex.Message);
                }
                return BadRequest(ex.Message);
            }
        }

    }
}
