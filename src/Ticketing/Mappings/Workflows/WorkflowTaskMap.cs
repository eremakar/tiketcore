using Data.Mapping;
using Data.Repository.Helpers;
using Ticketing.Data.TicketDb.Entities.Workflows;
using Ticketing.Models.Dtos.Workflows;
using Newtonsoft.Json;
using Data.Repository.Helpers;

namespace Ticketing.Mappings.Workflows
{
    /// <summary>
    /// Задача рабочего процесса
    /// </summary>
    public partial class WorkflowTaskMap : MapBase2<WorkflowTask, WorkflowTaskDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public WorkflowTaskMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override WorkflowTaskDto MapCore(WorkflowTask source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new WorkflowTaskDto();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Name = source.Name;
                result.Category = source.Category;
                result.Input = source.Input;
                result.Output = source.Output;
                result.StartTime = source.StartTime;
                result.EndTime = source.EndTime;
                result.ErrorMessage = source.ErrorMessage;
                result.State = source.State;
                result.Priority = source.Priority;
                result.RetryCount = source.RetryCount;
                result.MaxRetries = source.MaxRetries;
                result.ScheduledStartTime = source.ScheduledStartTime;
                result.Context = source.Context;
                result.ParentTaskId = source.ParentTaskId;
                result.UserId = source.UserId;
            }
            if (options.MapObjects)
            {
                result.ParentTask = mapContext.WorkflowTaskMap.Map(source.ParentTask, options);
                result.User = mapContext.UserMap.Map(source.User, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override WorkflowTask ReverseMapCore(WorkflowTaskDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new WorkflowTask();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Name = source.Name;
                result.Category = source.Category;
                if (source.Input != null)
                    result.Input = JsonConvert.SerializeObject(source.Input);
                if (source.Output != null)
                    result.Output = JsonConvert.SerializeObject(source.Output);
                result.StartTime = source.StartTime.ToUtc();
                result.EndTime = source.EndTime.ToUtc();
                result.ErrorMessage = source.ErrorMessage;
                result.State = source.State;
                result.Priority = source.Priority;
                result.RetryCount = source.RetryCount;
                result.MaxRetries = source.MaxRetries;
                result.ScheduledStartTime = source.ScheduledStartTime.ToUtc();
                if (source.Context != null)
                    result.Context = JsonConvert.SerializeObject(source.Context);
                result.ParentTaskId = source.ParentTaskId;
                result.UserId = source.UserId;
            }
            if (options.MapObjects)
            {
                if (source.ParentTaskId == null)
                    result.ParentTask = mapContext.WorkflowTaskMap.ReverseMap(source.ParentTask, options);
                if (source.UserId == null)
                    result.User = mapContext.UserMap.ReverseMap(source.User, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override void MapCore(WorkflowTask source, WorkflowTask destination, MapOptions options = null)
        {
            if (source == null || destination == null)
                return;

            options = options ?? new MapOptions();

            destination.Id = source.Id;
            if (options.MapProperties)
            {
                destination.Name = source.Name;
                destination.Category = source.Category;
                destination.Input = JsonHelper.NormalizeSafe(source.Input);
                destination.Output = JsonHelper.NormalizeSafe(source.Output);
                destination.StartTime = source.StartTime;
                destination.EndTime = source.EndTime;
                destination.ErrorMessage = source.ErrorMessage;
                destination.State = source.State;
                destination.Priority = source.Priority;
                destination.RetryCount = source.RetryCount;
                destination.MaxRetries = source.MaxRetries;
                destination.ScheduledStartTime = source.ScheduledStartTime;
                destination.Context = JsonHelper.NormalizeSafe(source.Context);
                destination.ParentTaskId = source.ParentTaskId;
                destination.UserId = source.UserId;
            }
            if (options.MapObjects)
            {
            }
            if (options.MapCollections)
            {
            }

        }
    }
}
