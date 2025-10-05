using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.Carriers
{
    /// <summary>
    /// Перевозчик
    /// </summary>
    public partial class CarrierSort : SortBase<Carrier>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? Name { get; set; }
        public SortOperand? BIN { get; set; }
        public SortOperand? Logo { get; set; }
    }
}
