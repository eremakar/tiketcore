using Data.Repository;
using Ticketing.Data.TicketDb.Entities.Workflows;

namespace Ticketing.Models.Queries.Workflows.WorkflowTasks
{
    /// <summary>
    /// Задача рабочего процесса
    /// </summary>
    public partial class WorkflowTaskSort : SortBase<WorkflowTask>
    {
        public SortOperand? Id { get; set; }
        /// <summary>
        /// Название
        /// </summary>
        public SortOperand? Name { get; set; }
        /// <summary>
        /// Категория
        /// </summary>
        public SortOperand? Category { get; set; }
        /// <summary>
        /// Входные данные
        /// </summary>
        public SortOperand? Input { get; set; }
        /// <summary>
        /// Выходные данные
        /// </summary>
        public SortOperand? Output { get; set; }
        /// <summary>
        /// Время начала
        /// </summary>
        public SortOperand? StartTime { get; set; }
        /// <summary>
        /// Время окончания
        /// </summary>
        public SortOperand? EndTime { get; set; }
        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public SortOperand? ErrorMessage { get; set; }
        /// <summary>
        /// Состояние: 1 - не начата, 2 - выполняется, 3 - завершена, 4 - завершена с ошибкой, 5 - отменена, 6 - приостановлена
        /// </summary>
        public SortOperand? State { get; set; }
        /// <summary>
        /// Приоритет
        /// </summary>
        public SortOperand? Priority { get; set; }
        /// <summary>
        /// Количество попыток перезапуска
        /// </summary>
        public SortOperand? RetryCount { get; set; }
        /// <summary>
        /// Максимум попыток
        /// </summary>
        public SortOperand? MaxRetries { get; set; }
        /// <summary>
        /// Запланированное время запуска
        /// </summary>
        public SortOperand? ScheduledStartTime { get; set; }
        /// <summary>
        /// Контекст выполнения
        /// </summary>
        public SortOperand? Context { get; set; }
        /// <summary>
        /// Родительская задача
        /// </summary>
        public SortOperand? ParentTaskId { get; set; }
        public SortOperand? UserId { get; set; }
    }
}
