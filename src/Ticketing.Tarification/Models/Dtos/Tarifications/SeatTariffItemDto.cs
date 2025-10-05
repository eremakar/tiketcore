using Ticketing.Tarifications.Models.Dtos;

namespace Ticketing.Tarifications.Models.Dtos.Tarifications
{
    /// <summary>
    /// Элемент тарифа места
    /// </summary>
    public partial class SeatTariffItemDto
    {
        public long Id { get; set; }
        public object? CalculationParameters { get; set; }
        public double Distance { get; set; }
        public double Price { get; set; }
        public long? WagonClassId { get; set; }
        public long? SeasonId { get; set; }
        public long? WagonId { get; set; }
        public long? SeatTypeId { get; set; }
        public long? FromId { get; set; }
        public long? ToId { get; set; }
        public long? SeatTariffId { get; set; }

        public WagonClassDto? WagonClass { get; set; }
        public SeasonDto? Season { get; set; }
        public WagonModelDto? Wagon { get; set; }
        public SeatTypeDto? SeatType { get; set; }
        public StationDto? From { get; set; }
        public StationDto? To { get; set; }
        public SeatTariffDto? SeatTariff { get; set; }
    }
}
