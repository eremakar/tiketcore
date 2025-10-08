using Data.Repository;
using Ticketing.Data.TicketDb.Entities.Dictionaries;

namespace Ticketing.Models.Queries.Dictionaries.TrainTypes
{
    /// <summary>
    /// Тип поезда
    /// </summary>
    public partial class TrainTypeFilter : FilterBase<TrainType>
    {
        public FilterOperand<long>? Id { get; set; }
        public FilterOperand<string>? Name { get; set; }
        public FilterOperand<string>? Code { get; set; }
    }
}
