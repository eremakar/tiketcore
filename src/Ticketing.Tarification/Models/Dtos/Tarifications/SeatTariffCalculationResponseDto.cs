namespace Ticketing.Tarifications.Models.Dtos.Tarifications
{
    /// <summary>
    /// Ответ с рассчитанными тарифами мест
    /// </summary>
    public class SeatTariffCalculationResponseDto
    {
        /// <summary>
        /// ID тарифа места
        /// </summary>
        public long SeatTariffId { get; set; }

        /// <summary>
        /// Рассчитанные элементы тарифа
        /// </summary>
        public List<SeatTariffItemCalculationDto> Items { get; set; } = new();
    }

    /// <summary>
    /// Рассчитанный элемент тарифа места
    /// </summary>
    public class SeatTariffItemCalculationDto
    {
        /// <summary>
        /// ID станции отправления
        /// </summary>
        public long FromStationId { get; set; }

        /// <summary>
        /// Название станции отправления
        /// </summary>
        public string? FromStationName { get; set; }

        /// <summary>
        /// ID станции прибытия
        /// </summary>
        public long ToStationId { get; set; }

        /// <summary>
        /// Название станции прибытия
        /// </summary>
        public string? ToStationName { get; set; }

        /// <summary>
        /// Расстояние между станциями
        /// </summary>
        public double Distance { get; set; }

        /// <summary>
        /// Рассчитанная цена
        /// </summary>
        public double Price { get; set; }
    }
}
