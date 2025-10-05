using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.SeatPurposes
{
    /// <summary>
    /// Назначение места
    /// </summary>
    public partial class SeatPurposeSort : SortBase<SeatPurpose>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? Name { get; set; }
        public SortOperand? Code { get; set; }
    }
}
