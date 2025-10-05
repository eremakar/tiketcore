using Data.Repository;
using Ticketing.Data.TicketDb.Entities.Workflows;

namespace Ticketing.Models.Queries.Workflows.WorkflowTasks
{
    public partial class WorkflowTaskQuery : QueryBase<WorkflowTask, WorkflowTaskFilter, WorkflowTaskSort>
    {
    }
}
