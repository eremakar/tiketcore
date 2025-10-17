using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Mappings;
using Ticketing.Models.Dtos;
using Ticketing.Models.Queries.TrainSchedules;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;
using Api.AspNetCore.Exceptions;

namespace Ticketing.Controllers
{
    public partial class TrainSchedulesController
    {
        /// <summary>
        /// Add new trainSchedule
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">Unique registered id</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/trainSchedules")]
        [HttpPost]
        [ProducesResponseType(typeof(TrainScheduleActivationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<object> AddAsync([FromBody] TrainScheduleDto request)
        {
            return await base.AddAsync(request);
        }

        /// <summary>
        /// Update trainSchedule
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">OK</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/trainSchedules")]
        [HttpPut]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<object> UpdateAsync([FromBody] TrainScheduleDto request)
        {
            return await base.UpdateAsync(request);
        }

        /// <summary>
        /// Patch trainSchedule
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">OK</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/trainSchedules/patch")]
        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<IActionResult> PatchAsync(long id, [FromBody] JsonPatchDocument<TrainScheduleDto> patch)
        {
            return await base.PatchAsync(id, patch);
        }

        /// <summary>
        /// Remove trainSchedule
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">OK</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/trainSchedules/{key}")]
        [HttpDelete]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<object> RemoveAsync([FromRoute] long key)
        {
            // Remove related TrainWagons and SeatSegments first
            var trainWagons = await restDb.Set<TrainWagon>()
                .Where(tw => tw.TrainScheduleId == key)
                .ToListAsync();

            if (trainWagons.Any())
            {
                var trainWagonIds = trainWagons.Select(tw => tw.Id).ToList();

                // Remove SeatSegments for these wagons
                var seatSegments = await restDb.Set<SeatSegment>()
                    .Where(ss => ss.TrainScheduleId == key)
                    .ToListAsync();

                if (seatSegments.Any())
                {
                    restDb.Set<SeatSegment>().RemoveRange(seatSegments);
                }

                // Remove TrainWagons
                restDb.Set<TrainWagon>().RemoveRange(trainWagons);
                await restDb.SaveChangesAsync();
            }

            return await base.RemoveAsync(key);
        }

        /// <summary>
        /// Activate trainSchedule: ensure Seats exist for each TrainWagon and build SeatSegments along route
        /// </summary>
        /// <remarks>
        /// For each wagon of the schedule, creates Seats 1..SeatCount if missing, then for all seats builds
        /// consecutive station segments (A-B, B-C, ...) from the train's route stations.
        /// </remarks>
        /// <response code="200">Activation summary</response>
        /// <response code="400">Invalid schedule or route</response>
        [Route("/api/v1/trainSchedules/{key}/activate")]
        [HttpPost]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> ActivateAsync([FromRoute] long key)
        {
            try
            {
                var result = await trainSchedulesService.Activate(key);
                return Ok(result);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Create train schedules for multiple dates with full seat and segment generation
        /// </summary>
        /// <remarks>
        /// Creates TrainSchedule for each date, then for each TrainWagon from Train.Plan.Wagons,
        /// creates Seats based on Wagon.SeatCount, and finally creates SeatSegments for all station pairs.
        /// </remarks>
        /// <response code="200">Creation summary with counts</response>
        /// <response code="400">Invalid train, plan, or route</response>
        [Route("/api/v1/trainSchedules/dates")]
        [HttpPost]
        [ProducesResponseType(typeof(TrainScheduleDatesResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> CreateSchedulesByDatesAsync([FromBody] TrainScheduleDatesRequestDto request)
        {
            try
            {
                var result = await trainSchedulesService.CreateSchedulesByDatesAsync(request);
                return Ok(result);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
