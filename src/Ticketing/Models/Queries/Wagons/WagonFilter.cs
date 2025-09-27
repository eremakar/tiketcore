using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.Wagons
{
    /// <summary>
    /// Вагон (тип)
    /// </summary>
    public partial class WagonFilter : FilterBase<Wagon>
    {
        public FilterOperand<long>? Id { get; set; }
        public FilterOperand<int>? SeatCount { get; set; }
        public FilterOperand<object>? PictureS3 { get; set; }
        public FilterOperand<string>? Class { get; set; }
        public FilterOperand<long?>? TypeId { get; set; }
    }
}
