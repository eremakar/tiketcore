using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities.Tarifications;

namespace Ticketing.Tarifications.Models.Queries.Tarifications.SeatTariffs
{
    /// <summary>
    /// Тариф места в вагоне
    /// </summary>
    public partial class SeatTariffFilter : FilterBase<SeatTariff>
    {
        public FilterOperand<long>? Id { get; set; }
        public FilterOperand<string>? Name { get; set; }
        public FilterOperand<long?>? TrainId { get; set; }
        public FilterOperand<long?>? BaseFareId { get; set; }
        public FilterOperand<long?>? TrainCategoryId { get; set; }
        public FilterOperand<long?>? TariffId { get; set; }
    }
}
