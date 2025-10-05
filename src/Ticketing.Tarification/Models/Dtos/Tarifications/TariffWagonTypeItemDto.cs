using Ticketing.Tarifications.Models.Dtos;

namespace Ticketing.Tarifications.Models.Dtos.Tarifications
{
    /// <summary>
    /// Элемент тарифа типа вагона
    /// </summary>
    public partial class TariffWagonTypeItemDto
    {
        public long Id { get; set; }
        public double IndexCoefficient { get; set; }
        public long? WagonTypeId { get; set; }
        public long? TariffId { get; set; }

        public WagonTypeDto? WagonType { get; set; }
        public TariffDto? Tariff { get; set; }
    }
}
