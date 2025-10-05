
namespace Ticketing.Models.Dtos.Workflows
{
    /// <summary>
    /// Лог задачи
    /// </summary>
    public partial class WorkflowTaskLogDto
    {
        public long Id { get; set; }
        /// <summary>
        /// Время
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// Сообщение
        /// </summary>
        public string? Message { get; set; }
        /// <summary>
        /// Данные
        /// </summary>
        public object? Data { get; set; }
        /// <summary>
        /// Стек вызовов
        /// </summary>
        public string? CallStack { get; set; }
        /// <summary>
        /// Уровень важности: 1 - info, 2 - warning, 3 - error, 4 - critical
        /// </summary>
        public int Severity { get; set; }
        /// <summary>
        /// Источник
        /// </summary>
        public string? Source { get; set; }
        public long? TaskId { get; set; }

        public WorkflowTaskDto? Task { get; set; }
    }
}
