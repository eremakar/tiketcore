using Data.Mapping;
using Data.Repository.Helpers;
using Ticketing.Data.TicketDb.Entities.Workflows;
using Ticketing.Models.Dtos.Workflows;
using Newtonsoft.Json;
using Data.Repository.Helpers;

namespace Ticketing.Mappings.Workflows
{
    /// <summary>
    /// Прогресс выполнения задачи
    /// </summary>
    public partial class WorkflowTaskProgressMap : MapBase2<WorkflowTaskProgress, WorkflowTaskProgressDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public WorkflowTaskProgressMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override WorkflowTaskProgressDto MapCore(WorkflowTaskProgress source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new WorkflowTaskProgressDto();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Percent = source.Percent;
                result.Time = source.Time;
                result.Message = source.Message;
                result.Data = source.Data;
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

        public override WorkflowTaskProgress ReverseMapCore(WorkflowTaskProgressDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new WorkflowTaskProgress();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Percent = source.Percent;
                result.Time = source.Time.ToUtc();
                result.Message = source.Message;
                if (source.Data != null)
                    result.Data = JsonConvert.SerializeObject(source.Data);
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

        public override void MapCore(WorkflowTaskProgress source, WorkflowTaskProgress destination, MapOptions options = null)
        {
            if (source == null || destination == null)
                return;

            options = options ?? new MapOptions();

            destination.Id = source.Id;
            if (options.MapProperties)
            {
                destination.Percent = source.Percent;
                destination.Time = source.Time;
                destination.Message = source.Message;
                destination.Data = JsonHelper.NormalizeSafe(source.Data);
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
