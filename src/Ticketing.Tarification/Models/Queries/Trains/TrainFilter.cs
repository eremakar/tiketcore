using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;

namespace Ticketing.Tarifications.Models.Queries.Trains
{
    /// <summary>
    /// Поезд
    /// </summary>
    public partial class TrainFilter : FilterBase<Train>
    {
        public FilterOperand<long>? Id { get; set; }
        public FilterOperand<string>? Name { get; set; }
        /// <summary>
        /// Тип поезда, например Тальго
        /// </summary>
        public FilterOperand<int>? Type { get; set; }
        /// <summary>
        /// Зональность: пригородный, общий и т.п.
        /// </summary>
        public FilterOperand<int>? ZoneType { get; set; }
        public FilterOperand<long?>? FromId { get; set; }
        public FilterOperand<long?>? ToId { get; set; }
        public FilterOperand<long?>? RouteId { get; set; }
        public FilterOperand<long?>? PlanId { get; set; }
        public FilterOperand<long?>? CategoryId { get; set; }
    }
}
