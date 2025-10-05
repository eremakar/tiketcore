using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities.Workflows;

namespace Ticketing.Services
{
    public enum WorkflowTaskState
    {
        NotStarted = 1,
        Running = 2,
        Completed = 3,
        Failed = 4,
        Cancelled = 5,
        Paused = 6
    }

    public enum LogSeverity
    {
        Info = 1,
        Warning = 2,
        Error = 3,
        Critical = 4
    }

    public class WorkflowTaskService
    {
        private readonly TicketDbContext db;
        private readonly ILogger<WorkflowTaskService> logger;

        public WorkflowTaskService(TicketDbContext db, ILogger<WorkflowTaskService> logger)
        {
            this.db = db;
            this.logger = logger;
        }

        /// <summary>
        /// Создает новую задачу с минимальными параметрами
        /// </summary>
        public async Task<long> CreateTaskAsync(string name, string? category = null)
        {
            var task = new WorkflowTask
            {
                Name = name,
                Category = category,
                State = (int)WorkflowTaskState.NotStarted,
                StartTime = DateTime.UtcNow,
                ScheduledStartTime = DateTime.UtcNow,
                Priority = 0,
                RetryCount = 0,
                MaxRetries = 3
            };

            db.WorkflowTasks!.Add(task);
            await db.SaveChangesAsync();

            logger.LogInformation($"Created workflow task {task.Id}: {name}");
            return task.Id;
        }

        /// <summary>
        /// Создает задачу с входными данными и контекстом
        /// </summary>
        public async Task<long> CreateTaskAsync<TInput>(
            string name,
            TInput input,
            string? category = null,
            int priority = 0)
        {
            var task = new WorkflowTask
            {
                Name = name,
                Category = category,
                Input = JsonSerializer.Serialize(input),
                State = (int)WorkflowTaskState.NotStarted,
                StartTime = DateTime.UtcNow,
                ScheduledStartTime = DateTime.UtcNow,
                Priority = priority,
                RetryCount = 0,
                MaxRetries = 3
            };

            db.WorkflowTasks!.Add(task);
            await db.SaveChangesAsync();

            logger.LogInformation($"Created workflow task {task.Id}: {name} with input");
            return task.Id;
        }

        /// <summary>
        /// Создает задачу с полным набором параметров
        /// </summary>
        public async Task<long> CreateTaskAsync<TInput, TContext>(
            string name,
            TInput input,
            TContext context,
            string? category = null,
            int priority = 0,
            int maxRetries = 3,
            DateTime? scheduledStartTime = null,
            long? parentTaskId = null,
            int? userId = null)
        {
            var task = new WorkflowTask
            {
                Name = name,
                Category = category,
                Input = JsonSerializer.Serialize(input),
                Context = JsonSerializer.Serialize(context),
                State = (int)WorkflowTaskState.NotStarted,
                StartTime = DateTime.UtcNow,
                ScheduledStartTime = scheduledStartTime ?? DateTime.UtcNow,
                Priority = priority,
                RetryCount = 0,
                MaxRetries = maxRetries,
                ParentTaskId = parentTaskId,
                UserId = userId
            };

            db.WorkflowTasks!.Add(task);
            await db.SaveChangesAsync();

            logger.LogInformation($"Created workflow task {task.Id}: {name} (full params)");
            return task.Id;
        }

        /// <summary>
        /// Создает дочернюю задачу
        /// </summary>
        public async Task<long> CreateChildTaskAsync(
            long parentTaskId,
            string name,
            string? category = null)
        {
            var task = new WorkflowTask
            {
                Name = name,
                Category = category,
                ParentTaskId = parentTaskId,
                State = (int)WorkflowTaskState.NotStarted,
                StartTime = DateTime.UtcNow,
                ScheduledStartTime = DateTime.UtcNow,
                Priority = 0,
                RetryCount = 0,
                MaxRetries = 3
            };

            db.WorkflowTasks!.Add(task);
            await db.SaveChangesAsync();

            await LogAsync(parentTaskId, $"Created child task: {name} (Id: {task.Id})", LogSeverity.Info);

            return task.Id;
        }

        /// <summary>
        /// Записывает лог задачи
        /// </summary>
        public async Task LogAsync(
            long? taskId,
            string message,
            LogSeverity severity = LogSeverity.Info,
            object? data = null,
            string? source = null,
            string? callStack = null)
        {
            if (!taskId.HasValue) return;

            var log = new WorkflowTaskLog
            {
                TaskId = taskId.Value,
                Time = DateTime.UtcNow,
                Message = message,
                Severity = (int)severity,
                Data = data != null ? JsonSerializer.Serialize(data) : null,
                Source = source,
                CallStack = callStack
            };

            db.WorkflowTaskLogs!.Add(log);
            await db.SaveChangesAsync();

            // Дополнительно логируем в основной logger
            switch (severity)
            {
                case LogSeverity.Info:
                    logger.LogInformation($"[Task {taskId}] {message}");
                    break;
                case LogSeverity.Warning:
                    logger.LogWarning($"[Task {taskId}] {message}");
                    break;
                case LogSeverity.Error:
                    logger.LogError($"[Task {taskId}] {message}");
                    break;
                case LogSeverity.Critical:
                    logger.LogCritical($"[Task {taskId}] {message}");
                    break;
            }
        }

        /// <summary>
        /// Записывает прогресс выполнения задачи
        /// </summary>
        public async Task UpdateProgressAsync(
            long? taskId,
            int percent,
            string? message = null,
            object? data = null)
        {
            if (!taskId.HasValue) return;

            var progress = new WorkflowTaskProgress
            {
                TaskId = taskId.Value,
                Percent = percent,
                Time = DateTime.UtcNow,
                Message = message,
                Data = data != null ? JsonSerializer.Serialize(data) : null
            };

            db.WorkflowTaskProgresses!.Add(progress);
            await db.SaveChangesAsync();

            logger.LogInformation($"[Task {taskId}] Progress: {percent}% - {message}");
        }

        /// <summary>
        /// Обновляет состояние задачи
        /// </summary>
        public async Task UpdateStateAsync(long? taskId, WorkflowTaskState state)
        {
            if (!taskId.HasValue) return;

            var task = await db.WorkflowTasks!.FindAsync(taskId.Value);
            if (task == null)
                throw new InvalidOperationException($"Task {taskId} not found");

            task.State = (int)state;
            
            if (state == WorkflowTaskState.Running && task.StartTime == default)
            {
                task.StartTime = DateTime.UtcNow;
            }
            
            if (state == WorkflowTaskState.Completed || state == WorkflowTaskState.Failed || state == WorkflowTaskState.Cancelled)
            {
                task.EndTime = DateTime.UtcNow;
            }

            await db.SaveChangesAsync();
            await LogAsync(taskId, $"State changed to {state}", LogSeverity.Info);
        }

        /// <summary>
        /// Запускает задачу
        /// </summary>
        public async Task StartTaskAsync(long? taskId)
        {
            if (!taskId.HasValue) return;
            
            await UpdateStateAsync(taskId, WorkflowTaskState.Running);
            await UpdateProgressAsync(taskId, 0, "Task started");
        }

        /// <summary>
        /// Завершает задачу успешно
        /// </summary>
        public async Task CompleteTaskAsync<TOutput>(long? taskId, TOutput output, string? message = null)
        {
            if (!taskId.HasValue) return;

            var task = await db.WorkflowTasks!.FindAsync(taskId.Value);
            if (task == null)
                throw new InvalidOperationException($"Task {taskId} not found");

            task.State = (int)WorkflowTaskState.Completed;
            task.Output = JsonSerializer.Serialize(output);
            task.EndTime = DateTime.UtcNow;

            await db.SaveChangesAsync();
            
            await UpdateProgressAsync(taskId, 100, message ?? "Task completed successfully");
            await LogAsync(taskId, message ?? "Task completed successfully", LogSeverity.Info);
        }

        /// <summary>
        /// Завершает задачу без output
        /// </summary>
        public async Task CompleteTaskAsync(long? taskId, string? message = null)
        {
            if (!taskId.HasValue) return;

            var task = await db.WorkflowTasks!.FindAsync(taskId.Value);
            if (task == null)
                throw new InvalidOperationException($"Task {taskId} not found");

            task.State = (int)WorkflowTaskState.Completed;
            task.EndTime = DateTime.UtcNow;

            await db.SaveChangesAsync();
            
            await UpdateProgressAsync(taskId, 100, message ?? "Task completed successfully");
            await LogAsync(taskId, message ?? "Task completed successfully", LogSeverity.Info);
        }

        /// <summary>
        /// Завершает задачу с ошибкой
        /// </summary>
        public async Task FailTaskAsync(long? taskId, string errorMessage, Exception? exception = null)
        {
            if (!taskId.HasValue) return;

            var task = await db.WorkflowTasks!.FindAsync(taskId.Value);
            if (task == null)
                throw new InvalidOperationException($"Task {taskId} not found");

            task.State = (int)WorkflowTaskState.Failed;
            task.ErrorMessage = errorMessage;
            task.EndTime = DateTime.UtcNow;

            await db.SaveChangesAsync();

            await LogAsync(
                taskId,
                errorMessage,
                LogSeverity.Error,
                data: exception != null ? new { ExceptionType = exception.GetType().Name, Message = exception.Message } : null,
                callStack: exception?.StackTrace
            );
        }

        /// <summary>
        /// Отменяет задачу
        /// </summary>
        public async Task CancelTaskAsync(long? taskId, string? reason = null)
        {
            if (!taskId.HasValue) return;
            
            await UpdateStateAsync(taskId, WorkflowTaskState.Cancelled);
            await LogAsync(taskId, $"Task cancelled. Reason: {reason ?? "No reason provided"}", LogSeverity.Warning);
        }

        /// <summary>
        /// Приостанавливает задачу
        /// </summary>
        public async Task PauseTaskAsync(long? taskId, string? reason = null)
        {
            if (!taskId.HasValue) return;
            
            await UpdateStateAsync(taskId, WorkflowTaskState.Paused);
            await LogAsync(taskId, $"Task paused. Reason: {reason ?? "No reason provided"}", LogSeverity.Info);
        }

        /// <summary>
        /// Возобновляет задачу
        /// </summary>
        public async Task ResumeTaskAsync(long? taskId)
        {
            if (!taskId.HasValue) return;
            
            await UpdateStateAsync(taskId, WorkflowTaskState.Running);
            await LogAsync(taskId, "Task resumed", LogSeverity.Info);
        }

        /// <summary>
        /// Повторяет задачу
        /// </summary>
        public async Task<bool> RetryTaskAsync(long? taskId)
        {
            if (!taskId.HasValue) return false;

            var task = await db.WorkflowTasks!.FindAsync(taskId.Value);
            if (task == null)
                throw new InvalidOperationException($"Task {taskId} not found");

            if (task.RetryCount >= task.MaxRetries)
            {
                await LogAsync(taskId, $"Max retries ({task.MaxRetries}) exceeded", LogSeverity.Error);
                return false;
            }

            task.RetryCount++;
            task.State = (int)WorkflowTaskState.Running;
            task.ErrorMessage = null;
            task.StartTime = DateTime.UtcNow;
            task.EndTime = default;

            await db.SaveChangesAsync();
            await LogAsync(taskId, $"Retry attempt {task.RetryCount}/{task.MaxRetries}", LogSeverity.Info);

            return true;
        }

        /// <summary>
        /// Получает информацию о задаче
        /// </summary>
        public async Task<WorkflowTask?> GetTaskAsync(long? taskId)
        {
            if (!taskId.HasValue) return null;

            return await db.WorkflowTasks!
                .Include(t => t.ParentTask)
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == taskId.Value);
        }

        /// <summary>
        /// Получает все логи задачи
        /// </summary>
        public async Task<List<WorkflowTaskLog>> GetLogsAsync(long? taskId)
        {
            if (!taskId.HasValue) return new List<WorkflowTaskLog>();

            return await db.WorkflowTaskLogs!
                .Where(l => l.TaskId == taskId.Value)
                .OrderBy(l => l.Time)
                .ToListAsync();
        }

        /// <summary>
        /// Получает прогресс задачи
        /// </summary>
        public async Task<List<WorkflowTaskProgress>> GetProgressAsync(long? taskId)
        {
            if (!taskId.HasValue) return new List<WorkflowTaskProgress>();

            return await db.WorkflowTaskProgresses!
                .Where(p => p.TaskId == taskId.Value)
                .OrderBy(p => p.Time)
                .ToListAsync();
        }

        /// <summary>
        /// Получает последний прогресс задачи
        /// </summary>
        public async Task<WorkflowTaskProgress?> GetLatestProgressAsync(long? taskId)
        {
            if (!taskId.HasValue) return null;

            return await db.WorkflowTaskProgresses!
                .Where(p => p.TaskId == taskId.Value)
                .OrderByDescending(p => p.Time)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Получает дочерние задачи
        /// </summary>
        public async Task<List<WorkflowTask>> GetChildTasksAsync(long? parentTaskId)
        {
            if (!parentTaskId.HasValue) return new List<WorkflowTask>();

            return await db.WorkflowTasks!
                .Where(t => t.ParentTaskId == parentTaskId.Value)
                .ToListAsync();
        }

        /// <summary>
        /// Обновляет контекст задачи
        /// </summary>
        public async Task UpdateContextAsync<TContext>(long? taskId, TContext context)
        {
            if (!taskId.HasValue) return;

            var task = await db.WorkflowTasks!.FindAsync(taskId.Value);
            if (task == null)
                throw new InvalidOperationException($"Task {taskId} not found");

            task.Context = JsonSerializer.Serialize(context);
            await db.SaveChangesAsync();
        }

        /// <summary>
        /// Получает контекст задачи
        /// </summary>
        public async Task<TContext?> GetContextAsync<TContext>(long? taskId)
        {
            if (!taskId.HasValue) return default;

            var task = await db.WorkflowTasks!.FindAsync(taskId.Value);
            if (task == null || string.IsNullOrEmpty(task.Context))
                return default;

            return JsonSerializer.Deserialize<TContext>(task.Context);
        }

        /// <summary>
        /// Получает входные данные задачи
        /// </summary>
        public async Task<TInput?> GetInputAsync<TInput>(long? taskId)
        {
            if (!taskId.HasValue) return default;

            var task = await db.WorkflowTasks!.FindAsync(taskId.Value);
            if (task == null || string.IsNullOrEmpty(task.Input))
                return default;

            return JsonSerializer.Deserialize<TInput>(task.Input);
        }

        /// <summary>
        /// Получает выходные данные задачи
        /// </summary>
        public async Task<TOutput?> GetOutputAsync<TOutput>(long? taskId)
        {
            if (!taskId.HasValue) return default;

            var task = await db.WorkflowTasks!.FindAsync(taskId.Value);
            if (task == null || string.IsNullOrEmpty(task.Output))
                return default;

            return JsonSerializer.Deserialize<TOutput>(task.Output);
        }
    }
}

