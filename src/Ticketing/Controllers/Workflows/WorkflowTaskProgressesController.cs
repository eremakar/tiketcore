using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities.Workflows;
using Ticketing.Mappings.Workflows;
using Ticketing.Models.Dtos.Workflows;
using Ticketing.Models.Queries.Workflows.WorkflowTaskProgresses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;

namespace Ticketing.Controllers.Workflows
{
    /// <summary>
    /// Прогресс выполнения задачи
    /// </summary>
    [Route("/api/v1/workflowTaskProgresses")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class WorkflowTaskProgressesController : RestControllerBase2<WorkflowTaskProgress, long, WorkflowTaskProgressDto, WorkflowTaskProgressQuery, WorkflowTaskProgressMap>
    {
        public WorkflowTaskProgressesController(ILogger<RestServiceBase<WorkflowTaskProgress, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            WorkflowTaskProgressMap workflowTaskProgressMap)
            : base(logger,
                restDapperDb,
                restDb,
                "WorkflowTaskProgresses",
                workflowTaskProgressMap)
        {
        }

        /// <summary>
        /// Search of WorkflowTaskProgress using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of workflowTaskProgresses</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/workflowTaskProgresses/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<WorkflowTaskProgressDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<WorkflowTaskProgressDto>> SearchAsync([FromBody] WorkflowTaskProgressQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.Task));
        }

        /// <summary>
        /// Get the workflowTaskProgress by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">WorkflowTaskProgress data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/workflowTaskProgresses/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(WorkflowTaskProgressDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<WorkflowTaskProgressDto> FindAsync([FromRoute] long key)
        {
            return await FindUsingEfAsync(key, _ => _.
                Include(_ => _.Task));
        }

    }
}
