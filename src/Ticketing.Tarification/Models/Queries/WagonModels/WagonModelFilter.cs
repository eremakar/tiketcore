using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;

namespace Ticketing.Tarifications.Models.Queries.WagonModels
{
    /// <summary>
    /// Вагон (тип)
    /// </summary>
    public partial class WagonModelFilter : FilterBase<WagonModel>
    {
        public FilterOperand<long>? Id { get; set; }
        public FilterOperand<string>? Name { get; set; }
        public FilterOperand<int>? SeatCount { get; set; }
        public FilterOperand<object>? PictureS3 { get; set; }
        public FilterOperand<string>? Class { get; set; }
        public FilterOperand<long?>? TypeId { get; set; }
    }
}
