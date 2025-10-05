namespace Ticketing.Tarifications.Models.Dtos.Tarifications
{
    /// <summary>
    /// Параметры расчета тарифа места
    /// </summary>
    public class SeatTariffCalculationParameters
    {
        /// <summary>
        /// Формула расчета с конкретными значениями
        /// </summary>
        public string Expression { get; set; } = string.Empty;
    }
}

