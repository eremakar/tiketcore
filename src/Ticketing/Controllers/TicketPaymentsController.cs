using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Mappings;
using Ticketing.Models.Dtos;
using Ticketing.Models.Queries.TicketPayments;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;

namespace Ticketing.Controllers
{
    /// <summary>
    /// Оплата билета
    /// </summary>
    [Route("/api/v1/ticketPayments")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class TicketPaymentsController : RestControllerBase2<TicketPayment, long, TicketPaymentDto, TicketPaymentQuery, TicketPaymentMap>
    {
        public TicketPaymentsController(ILogger<RestServiceBase<TicketPayment, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            TicketPaymentMap ticketPaymentMap)
            : base(logger,
                restDapperDb,
                restDb,
                "TicketPayments",
                ticketPaymentMap)
        {
        }

        /// <summary>
        /// Search of TicketPayment using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of ticketPayments</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/ticketPayments/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<TicketPaymentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<TicketPaymentDto>> SearchAsync([FromBody] TicketPaymentQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.Ticket).
                Include(_ => _.User));
        }

        /// <summary>
        /// Get the ticketPayment by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">TicketPayment data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/ticketPayments/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(TicketPaymentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<TicketPaymentDto> FindAsync([FromRoute] long key)
        {
            return await FindUsingEfAsync(key, _ => _.
                Include(_ => _.Ticket).
                Include(_ => _.User));
        }

    }
}
