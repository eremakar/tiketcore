using Ticketing.Tarifications.Models.Dtos;

namespace Ticketing.Tarifications.Models.Dtos.Tarifications
{
    /// <summary>
    /// Элемент тарифа типа места
    /// </summary>
    public partial class TariffSeatTypeItemDto
    {
        public long Id { get; set; }
        public double IndexCoefficient { get; set; }
        public long? SeatTypeId { get; set; }
        public long? TariffWagonId { get; set; }

        public SeatTypeDto? SeatType { get; set; }
        public TariffWagonItemDto? TariffWagon { get; set; }
    }
}
