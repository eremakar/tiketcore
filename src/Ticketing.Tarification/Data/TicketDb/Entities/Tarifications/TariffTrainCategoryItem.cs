using Data.Repository;

namespace Ticketing.Tarifications.Data.TicketDb.Entities.Tarifications
{
    /// <summary>
    /// Элемент тарифа категории поезда
    /// </summary>
    public partial class TariffTrainCategoryItem : IEntityKey<long>
    {
        public long Id { get; set; }
        public double IndexCoefficient { get; set; }
        public long? TrainCategoryId { get; set; }
        public long? TariffId { get; set; }

        public TrainCategory? TrainCategory { get; set; }
        public Tariff? Tariff { get; set; }
    }
}
