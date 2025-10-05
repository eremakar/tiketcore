using Data.Repository;
using Ticketing.Data.TicketDb.Entities.Workflows;

namespace Ticketing.Models.Queries.Workflows.WorkflowTaskLogs
{
    /// <summary>
    /// Лог задачи
    /// </summary>
    public partial class WorkflowTaskLogSort : SortBase<WorkflowTaskLog>
    {
        public SortOperand? Id { get; set; }
        /// <summary>
        /// Время
        /// </summary>
        public SortOperand? Time { get; set; }
        /// <summary>
        /// Сообщение
        /// </summary>
        public SortOperand? Message { get; set; }
        /// <summary>
        /// Данные
        /// </summary>
        public SortOperand? Data { get; set; }
        /// <summary>
        /// Стек вызовов
        /// </summary>
        public SortOperand? CallStack { get; set; }
        /// <summary>
        /// Уровень важности: 1 - info, 2 - warning, 3 - error, 4 - critical
        /// </summary>
        public SortOperand? Severity { get; set; }
        /// <summary>
        /// Источник
        /// </summary>
        public SortOperand? Source { get; set; }
        public SortOperand? TaskId { get; set; }
    }
}
