namespace Ticketing.Tarifications.Models.Dtos.Tarifications
{
    /// <summary>
    /// Запрос на расчет тарифов мест
    /// </summary>
    public class SeatTariffCalculationRequestDto
    {
        /// <summary>
        /// ID тарифа места
        /// </summary>
        public long SeatTariffId { get; set; }
    }
}
