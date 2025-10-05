using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;

namespace Ticketing.Tarifications.Models.Queries.Trains
{
    /// <summary>
    /// Поезд
    /// </summary>
    public partial class TrainSort : SortBase<Train>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? Name { get; set; }
        /// <summary>
        /// Тип поезда, например Тальго
        /// </summary>
        public SortOperand? Type { get; set; }
        /// <summary>
        /// Зональность: пригородный, общий и т.п.
        /// </summary>
        public SortOperand? ZoneType { get; set; }
        public SortOperand? FromId { get; set; }
        public SortOperand? ToId { get; set; }
        public SortOperand? RouteId { get; set; }
        public SortOperand? PlanId { get; set; }
        public SortOperand? CategoryId { get; set; }
    }
}
