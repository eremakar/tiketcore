
namespace Ticketing.Tarifications.Models.Dtos.Tarifications
{
    /// <summary>
    /// Элемент тарифа категории поезда
    /// </summary>
    public partial class TariffTrainCategoryItemDto
    {
        public long Id { get; set; }
        public double IndexCoefficient { get; set; }
        public long? TrainCategoryId { get; set; }
        public long? TariffId { get; set; }

        public TrainCategoryDto? TrainCategory { get; set; }
        public TariffDto? Tariff { get; set; }
    }
}
