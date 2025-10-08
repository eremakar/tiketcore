using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.Trains
{
    /// <summary>
    /// Поезд по маршруту
    /// </summary>
    public partial class TrainFilter : FilterBase<Train>
    {
        public FilterOperand<long>? Id { get; set; }
        public FilterOperand<string>? Name { get; set; }
        /// <summary>
        /// Зональность: пригородный, общий и т.п.
        /// </summary>
        public FilterOperand<int>? ZoneType { get; set; }
        /// <summary>
        /// 1 - Социально-значимый, 2 - Коммерческий
        /// </summary>
        public FilterOperand<int>? Importance { get; set; }
        /// <summary>
        /// 1 - вагон-ресторан, 2 - вагон-бар
        /// </summary>
        public FilterOperand<int>? Amenities { get; set; }
        /// <summary>
        /// Тип поезда, например Тальго
        /// </summary>
        public FilterOperand<long?>? TypeId { get; set; }
        public FilterOperand<long?>? FromId { get; set; }
        public FilterOperand<long?>? ToId { get; set; }
        public FilterOperand<long?>? RouteId { get; set; }
        public FilterOperand<long?>? PeriodicityId { get; set; }
        public FilterOperand<long?>? PlanId { get; set; }
        public FilterOperand<long?>? CategoryId { get; set; }
    }
}
