using Data.Repository;
using Ticketing.Data.TicketDb.Entities.Workflows;

namespace Ticketing.Models.Queries.Workflows.WorkflowTaskProgresses
{
    /// <summary>
    /// Прогресс выполнения задачи
    /// </summary>
    public partial class WorkflowTaskProgressFilter : FilterBase<WorkflowTaskProgress>
    {
        public FilterOperand<long>? Id { get; set; }
        /// <summary>
        /// Процент выполнения
        /// </summary>
        public FilterOperand<int>? Percent { get; set; }
        /// <summary>
        /// Время
        /// </summary>
        public FilterOperand<DateTime>? Time { get; set; }
        /// <summary>
        /// Сообщение
        /// </summary>
        public FilterOperand<string>? Message { get; set; }
        /// <summary>
        /// Данные
        /// </summary>
        public FilterOperand<object>? Data { get; set; }
        public FilterOperand<long?>? TaskId { get; set; }
    }
}
