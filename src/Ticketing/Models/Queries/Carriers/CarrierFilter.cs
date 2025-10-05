using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.Carriers
{
    /// <summary>
    /// Перевозчик
    /// </summary>
    public partial class CarrierFilter : FilterBase<Carrier>
    {
        public FilterOperand<long>? Id { get; set; }
        public FilterOperand<string>? Name { get; set; }
        public FilterOperand<string>? BIN { get; set; }
        public FilterOperand<object>? Logo { get; set; }
    }
}
