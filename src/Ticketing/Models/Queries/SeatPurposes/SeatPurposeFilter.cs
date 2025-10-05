using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.SeatPurposes
{
    /// <summary>
    /// Назначение места
    /// </summary>
    public partial class SeatPurposeFilter : FilterBase<SeatPurpose>
    {
        public FilterOperand<long>? Id { get; set; }
        public FilterOperand<string>? Name { get; set; }
        public FilterOperand<string>? Code { get; set; }
    }
}
