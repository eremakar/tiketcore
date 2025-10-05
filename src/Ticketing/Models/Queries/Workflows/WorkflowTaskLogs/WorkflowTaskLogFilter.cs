using Data.Repository;
using Ticketing.Data.TicketDb.Entities.Workflows;

namespace Ticketing.Models.Queries.Workflows.WorkflowTaskLogs
{
    /// <summary>
    /// Лог задачи
    /// </summary>
    public partial class WorkflowTaskLogFilter : FilterBase<WorkflowTaskLog>
    {
        public FilterOperand<long>? Id { get; set; }
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
        /// <summary>
        /// Стек вызовов
        /// </summary>
        public FilterOperand<string>? CallStack { get; set; }
        /// <summary>
        /// Уровень важности: 1 - info, 2 - warning, 3 - error, 4 - critical
        /// </summary>
        public FilterOperand<int>? Severity { get; set; }
        /// <summary>
        /// Источник
        /// </summary>
        public FilterOperand<string>? Source { get; set; }
        public FilterOperand<long?>? TaskId { get; set; }
    }
}
