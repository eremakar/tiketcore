using Data.Repository;
using Data.Repository.Dapper;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities.Workflows;
using Ticketing.Mappings.Workflows;
using Ticketing.Models.Dtos.Workflows;
using Ticketing.Models.Queries.Workflows.WorkflowTasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;

namespace Ticketing.Controllers.Workflows
{
    /// <summary>
    /// Задача рабочего процесса
    /// </summary>
    [Route("/api/v1/workflowTasks")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdministrator,Administrator")]
    public partial class WorkflowTasksController : RestControllerBase2<WorkflowTask, long, WorkflowTaskDto, WorkflowTaskQuery, WorkflowTaskMap>
    {
        public WorkflowTasksController(ILogger<RestServiceBase<WorkflowTask, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            WorkflowTaskMap workflowTaskMap)
            : base(logger,
                restDapperDb,
                restDb,
                "WorkflowTasks",
                workflowTaskMap)
        {
        }

        /// <summary>
        /// Search of WorkflowTask using given query
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">List of workflowTasks</response>
        /// <response code="400">Validation errors detected, operation denied</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/workflowTasks/search")]
        [HttpPost]
        [ProducesResponseType(typeof(PagedList<WorkflowTaskDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<PagedList<WorkflowTaskDto>> SearchAsync([FromBody] WorkflowTaskQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.ParentTask).
                Include(_ => _.User));
        }

        /// <summary>
        /// Get the workflowTask by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">WorkflowTask data</response>
        /// <response code="401">Unauthorized request</response>
        [Route("/api/v1/workflowTasks/{key}")]
        [HttpGet]
        [ProducesResponseType(typeof(WorkflowTaskDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public override async Task<WorkflowTaskDto> FindAsync([FromRoute] long key)
        {
            return await FindUsingEfAsync(key, _ => _.
                Include(_ => _.ParentTask).
                Include(_ => _.User));
        }

    }
}
