using Data.Mapping;
using Data.Repository.Helpers;
using Ticketing.Data.TicketDb.Entities.Workflows;
using Ticketing.Models.Dtos.Workflows;
using Newtonsoft.Json;
using Data.Repository.Helpers;

namespace Ticketing.Mappings.Workflows
{
    /// <summary>
    /// Лог задачи
    /// </summary>
    public partial class WorkflowTaskLogMap : MapBase2<WorkflowTaskLog, WorkflowTaskLogDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public WorkflowTaskLogMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override WorkflowTaskLogDto MapCore(WorkflowTaskLog source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new WorkflowTaskLogDto();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Time = source.Time;
                result.Message = source.Message;
                result.Data = source.Data;
                result.CallStack = source.CallStack;
                result.Severity = source.Severity;
                result.Source = source.Source;
                result.TaskId = source.TaskId;
            }
            if (options.MapObjects)
            {
                result.Task = mapContext.WorkflowTaskMap.Map(source.Task, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override WorkflowTaskLog ReverseMapCore(WorkflowTaskLogDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new WorkflowTaskLog();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Time = source.Time.ToUtc();
                result.Message = source.Message;
                if (source.Data != null)
                    result.Data = JsonConvert.SerializeObject(source.Data);
                result.CallStack = source.CallStack;
                result.Severity = source.Severity;
                result.Source = source.Source;
                result.TaskId = source.TaskId;
            }
            if (options.MapObjects)
            {
                if (source.TaskId == null)
                    result.Task = mapContext.WorkflowTaskMap.ReverseMap(source.Task, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override void MapCore(WorkflowTaskLog source, WorkflowTaskLog destination, MapOptions options = null)
        {
            if (source == null || destination == null)
                return;

            options = options ?? new MapOptions();

            destination.Id = source.Id;
            if (options.MapProperties)
            {
                destination.Time = source.Time;
                destination.Message = source.Message;
                destination.Data = JsonHelper.NormalizeSafe(source.Data);
                destination.CallStack = source.CallStack;
                destination.Severity = source.Severity;
                destination.Source = source.Source;
                destination.TaskId = source.TaskId;
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
