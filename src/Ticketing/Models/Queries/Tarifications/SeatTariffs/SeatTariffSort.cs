using Data.Repository;
using Ticketing.Data.TicketDb.Entities.Tarifications;

namespace Ticketing.Models.Queries.Tarifications.SeatTariffs
{
    /// <summary>
    /// Тариф места в вагоне
    /// </summary>
    public partial class SeatTariffSort : SortBase<SeatTariff>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? Name { get; set; }
        public SortOperand? TrainId { get; set; }
        public SortOperand? BaseFareId { get; set; }
        public SortOperand? TrainCategoryId { get; set; }
    }
}
