using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.Trains
{
    /// <summary>
    /// Поезд по маршруту
    /// </summary>
    public partial class TrainSort : SortBase<Train>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? Name { get; set; }
        /// <summary>
        /// Зональность: пригородный, общий и т.п.
        /// </summary>
        public SortOperand? ZoneType { get; set; }
        /// <summary>
        /// 1 - Социально-значимый, 2 - Коммерческий
        /// </summary>
        public SortOperand? Importance { get; set; }
        /// <summary>
        /// 1 - вагон-ресторан, 2 - вагон-бар
        /// </summary>
        public SortOperand? Amenities { get; set; }
        /// <summary>
        /// Тип поезда, например Тальго
        /// </summary>
        public SortOperand? TypeId { get; set; }
        public SortOperand? FromId { get; set; }
        public SortOperand? ToId { get; set; }
        public SortOperand? RouteId { get; set; }
        public SortOperand? PeriodicityId { get; set; }
        public SortOperand? PlanId { get; set; }
        public SortOperand? CategoryId { get; set; }
    }
}
