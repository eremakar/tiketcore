using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;

namespace Ticketing.Tarifications.Models.Queries.WagonTypes
{
    /// <summary>
    /// Тип вагона
    /// </summary>
    public partial class WagonTypeSort : SortBase<WagonType>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? Name { get; set; }
        public SortOperand? ShortName { get; set; }
        public SortOperand? Code { get; set; }
    }
}
