using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities.Workflows;
using Ticketing.Mappings.Workflows;
using Ticketing.Models.Dtos.Workflows;
using Ticketing.Models.Queries.Workflows.WorkflowTaskLogs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;

namespace Ticketing.Controllers.Workflows
{
    /// <summary>
    /// Лог задачи
    /// </summary>
    [Route("/api/v1/workflowTaskLogs")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class WorkflowTaskLogsController : RestControllerBase2<WorkflowTaskLog, long, WorkflowTaskLogDto, WorkflowTaskLogQuery, WorkflowTaskLogMap>
    {
        public WorkflowTaskLogsController(ILogger<RestServiceBase<WorkflowTaskLog, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            WorkflowTaskLogMap workflowTaskLogMap)
            : base(logger,
                restDapperDb,
                restDb,
                "WorkflowTaskLogs",
                workflowTaskLogMap)
        {
        }

        /// <summary>
        /// Search of WorkflowTaskLog using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of workflowTaskLogs</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/workflowTaskLogs/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<WorkflowTaskLogDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<WorkflowTaskLogDto>> SearchAsync([FromBody] WorkflowTaskLogQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.Task));
        }

        /// <summary>
        /// Get the workflowTaskLog by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">WorkflowTaskLog data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/workflowTaskLogs/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(WorkflowTaskLogDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<WorkflowTaskLogDto> FindAsync([FromRoute] long key)
        {
            return await FindUsingEfAsync(key, _ => _.
                Include(_ => _.Task));
        }

    }
}
