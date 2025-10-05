using Ticketing.Models.Dtos;

namespace Ticketing.Models.Dtos.Tarifications
{
    /// <summary>
    /// Элемент тарифа вагона
    /// </summary>
    public partial class TariffWagonItemDto
    {
        public long Id { get; set; }
        public double IndexCoefficient { get; set; }
        public long? WagonId { get; set; }
        public long? TariffId { get; set; }

        public WagonModelDto? Wagon { get; set; }
        public TariffDto? Tariff { get; set; }

        public List<TariffSeatTypeItemDto>? SeatTypes { get; set; }
    }
}
