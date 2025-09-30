using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities.Tarifications;

namespace Ticketing.Tarifications.Models.Queries.Tarifications.Tariffs
{
    /// <summary>
    /// Тариф
    /// </summary>
    public partial class TariffSort : SortBase<Tariff>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? Name { get; set; }
        public SortOperand? IndexCoefficient { get; set; }
        public SortOperand? VAT { get; set; }
        public SortOperand? BaseFareId { get; set; }
        public SortOperand? TrainCategoryId { get; set; }
        public SortOperand? WagonId { get; set; }
    }
}
