using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.Filials
{
    /// <summary>
    /// Филиал
    /// </summary>
    public partial class FilialSort : SortBase<Filial>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? Name { get; set; }
        public SortOperand? Code { get; set; }
    }
}
