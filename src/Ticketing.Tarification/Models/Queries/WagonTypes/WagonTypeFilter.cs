using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;

namespace Ticketing.Tarifications.Models.Queries.WagonTypes
{
    /// <summary>
    /// Тип вагона
    /// </summary>
    public partial class WagonTypeFilter : FilterBase<WagonType>
    {
        public FilterOperand<long>? Id { get; set; }
        public FilterOperand<string>? Name { get; set; }
        public FilterOperand<string>? ShortName { get; set; }
        public FilterOperand<string>? Code { get; set; }
    }
}
