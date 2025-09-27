using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.TrainWagons
{
    /// <summary>
    /// Вагон состава поезда
    /// </summary>
    public partial class TrainWagonSort : SortBase<TrainWagon>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? Number { get; set; }
        public SortOperand? TrainScheduleId { get; set; }
        public SortOperand? WagonId { get; set; }
    }
}
