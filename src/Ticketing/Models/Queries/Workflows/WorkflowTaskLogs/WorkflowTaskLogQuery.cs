using Data.Repository;
using Ticketing.Data.TicketDb.Entities.Workflows;

namespace Ticketing.Models.Queries.Workflows.WorkflowTaskLogs
{
    public partial class WorkflowTaskLogQuery : QueryBase<WorkflowTaskLog, WorkflowTaskLogFilter, WorkflowTaskLogSort>
    {
    }
}
