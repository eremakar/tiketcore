using Data.Repository;
using Ticketing.Data.TicketDb.Entities.Dictionaries;

namespace Ticketing.Models.Queries.Dictionaries.TrainTypes
{
    /// <summary>
    /// Тип поезда
    /// </summary>
    public partial class TrainTypeSort : SortBase<TrainType>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? Name { get; set; }
        public SortOperand? Code { get; set; }
    }
}
