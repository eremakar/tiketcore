using Ticketing.Models.Dtos;

namespace Ticketing.Models.Dtos.Workflows
{
    /// <summary>
    /// Задача рабочего процесса
    /// </summary>
    public partial class WorkflowTaskDto
    {
        public long Id { get; set; }
        /// <summary>
        /// Название
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Категория
        /// </summary>
        public string? Category { get; set; }
        /// <summary>
        /// Входные данные
        /// </summary>
        public object? Input { get; set; }
        /// <summary>
        /// Выходные данные
        /// </summary>
        public object? Output { get; set; }
        /// <summary>
        /// Время начала
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// Время окончания
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public string? ErrorMessage { get; set; }
        /// <summary>
        /// Состояние: 1 - не начата, 2 - выполняется, 3 - завершена, 4 - завершена с ошибкой, 5 - отменена, 6 - приостановлена
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// Приоритет
        /// </summary>
        public int Priority { get; set; }
        /// <summary>
        /// Количество попыток перезапуска
        /// </summary>
        public int RetryCount { get; set; }
        /// <summary>
        /// Максимум попыток
        /// </summary>
        public int MaxRetries { get; set; }
        /// <summary>
        /// Запланированное время запуска
        /// </summary>
        public DateTime ScheduledStartTime { get; set; }
        /// <summary>
        /// Контекст выполнения
        /// </summary>
        public object? Context { get; set; }
        /// <summary>
        /// Родительская задача
        /// </summary>
        public long? ParentTaskId { get; set; }
        public int? UserId { get; set; }

        /// <summary>
        /// Родительская задача
        /// </summary>
        public WorkflowTaskDto? ParentTask { get; set; }
        public UserDto? User { get; set; }
    }
}
