using Data.Repository;
using Ticketing.Data.TicketDb.Entities.Workflows;

namespace Ticketing.Models.Queries.Workflows.WorkflowTaskProgresses
{
    /// <summary>
    /// Прогресс выполнения задачи
    /// </summary>
    public partial class WorkflowTaskProgressSort : SortBase<WorkflowTaskProgress>
    {
        public SortOperand? Id { get; set; }
        /// <summary>
        /// Процент выполнения
        /// </summary>
        public SortOperand? Percent { get; set; }
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
        public SortOperand? TaskId { get; set; }
    }
}
