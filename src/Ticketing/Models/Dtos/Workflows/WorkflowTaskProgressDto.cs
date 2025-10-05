
namespace Ticketing.Models.Dtos.Workflows
{
    /// <summary>
    /// Прогресс выполнения задачи
    /// </summary>
    public partial class WorkflowTaskProgressDto
    {
        public long Id { get; set; }
        /// <summary>
        /// Процент выполнения
        /// </summary>
        public int Percent { get; set; }
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
        public long? TaskId { get; set; }

        public WorkflowTaskDto? Task { get; set; }
    }
}
