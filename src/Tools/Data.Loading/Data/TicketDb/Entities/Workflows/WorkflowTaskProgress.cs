using System.Text.Json;
using Data.Repository;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketing.Data.TicketDb.Entities.Workflows
{
    /// <summary>
    /// Прогресс выполнения задачи
    /// </summary>
    public partial class WorkflowTaskProgress : IEntityKey<long>
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
        [Column(TypeName = "jsonb")]
        public string? Data { get; set; }
        public long? TaskId { get; set; }

        public WorkflowTask? Task { get; set; }
    }
}
