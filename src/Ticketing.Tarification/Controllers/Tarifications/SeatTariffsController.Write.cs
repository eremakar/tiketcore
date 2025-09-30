using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Tarifications.Data.TicketDb.DatabaseContext;
using Ticketing.Tarifications.Data.TicketDb.Entities.Tarifications;
using Ticketing.Tarifications.Mappings.Tarifications;
using Ticketing.Tarifications.Models.Dtos.Tarifications;
using Ticketing.Tarifications.Models.Queries.Tarifications.SeatTariffs;
using Ticketing.Tarifications.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;

namespace Ticketing.Tarifications.Controllers.Tarifications
{
    public partial class SeatTariffsController
    {
        /// <summary>
        /// Add new seatTariff
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">Unique registered id</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/seatTariffs")]
        [HttpPost]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<object> AddAsync([FromBody] SeatTariffDto request)
        {
            return await base.AddAsync(request);
        }

        /// <summary>
        /// Update seatTariff
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">OK</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/seatTariffs")]
        [HttpPut]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<object> UpdateAsync([FromBody] SeatTariffDto request)
        {
            return await base.UpdateAsync(request);
        }

        /// <summary>
        /// Patch seatTariff
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">OK</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/seatTariffs/patch")]
        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<IActionResult> PatchAsync(long id, [FromBody] JsonPatchDocument<SeatTariffDto> patch)
        {
            return await base.PatchAsync(id, patch);
        }

        /// <summary>
        /// Remove seatTariff
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">OK</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/seatTariffs/{key}")]
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
        /// Calculate seat tariff items for all station pairs
        /// </summary>
        /// <remarks>
        /// Calculates prices for all station pairs in the train route and saves them to database.
        /// Updates existing items or creates new ones if they don't exist.
        /// </remarks>
        /// <response code="200">Number of processed tariff items</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/seatTariffs/{key}/items/calculate")]
        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<int> CalculateItemsAsync([FromRoute] long key)
        {
            var seatTariffService = HttpContext.RequestServices.GetRequiredService<SeatTariffService>();
            return await seatTariffService.CalculateAsync(key);
        }

    }
}
