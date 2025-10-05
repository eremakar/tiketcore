using Data.Repository;
using Ticketing.Data.TicketDb.Entities.Workflows;

namespace Ticketing.Models.Queries.Workflows.WorkflowTasks
{
    /// <summary>
    /// Задача рабочего процесса
    /// </summary>
    public partial class WorkflowTaskFilter : FilterBase<WorkflowTask>
    {
        public FilterOperand<long>? Id { get; set; }
        /// <summary>
        /// Название
        /// </summary>
        public FilterOperand<string>? Name { get; set; }
        /// <summary>
        /// Категория
        /// </summary>
        public FilterOperand<string>? Category { get; set; }
        /// <summary>
        /// Входные данные
        /// </summary>
        public FilterOperand<object>? Input { get; set; }
        /// <summary>
        /// Выходные данные
        /// </summary>
        public FilterOperand<object>? Output { get; set; }
        /// <summary>
        /// Время начала
        /// </summary>
        public FilterOperand<DateTime>? StartTime { get; set; }
        /// <summary>
        /// Время окончания
        /// </summary>
        public FilterOperand<DateTime>? EndTime { get; set; }
        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public FilterOperand<string>? ErrorMessage { get; set; }
        /// <summary>
        /// Состояние: 1 - не начата, 2 - выполняется, 3 - завершена, 4 - завершена с ошибкой, 5 - отменена, 6 - приостановлена
        /// </summary>
        public FilterOperand<int>? State { get; set; }
        /// <summary>
        /// Приоритет
        /// </summary>
        public FilterOperand<int>? Priority { get; set; }
        /// <summary>
        /// Количество попыток перезапуска
        /// </summary>
        public FilterOperand<int>? RetryCount { get; set; }
        /// <summary>
        /// Максимум попыток
        /// </summary>
        public FilterOperand<int>? MaxRetries { get; set; }
        /// <summary>
        /// Запланированное время запуска
        /// </summary>
        public FilterOperand<DateTime>? ScheduledStartTime { get; set; }
        /// <summary>
        /// Контекст выполнения
        /// </summary>
        public FilterOperand<object>? Context { get; set; }
        /// <summary>
        /// Родительская задача
        /// </summary>
        public FilterOperand<long?>? ParentTaskId { get; set; }
        public FilterOperand<int?>? UserId { get; set; }
    }
}
