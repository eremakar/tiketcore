using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities.Tarifications;

namespace Ticketing.Tarifications.Models.Queries.Tarifications.Tariffs
{
    /// <summary>
    /// Тариф
    /// </summary>
    public partial class TariffFilter : FilterBase<Tariff>
    {
        public FilterOperand<long>? Id { get; set; }
        public FilterOperand<string>? Name { get; set; }
        public FilterOperand<double>? IndexCoefficient { get; set; }
        public FilterOperand<double>? VAT { get; set; }
        public FilterOperand<long?>? BaseFareId { get; set; }
        public FilterOperand<long?>? TrainCategoryId { get; set; }
        public FilterOperand<long?>? WagonId { get; set; }
    }
}
