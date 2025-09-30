using Ticketing.Tarifications.Models.Dtos.Tarifications;

namespace Ticketing.Tarifications.Models.Dtos
{
    /// <summary>
    /// Расписание поезда по дням
    /// </summary>
    public partial class TrainScheduleDto
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public bool Active { get; set; }
        public long? TrainId { get; set; }
        public long? SeatTariffId { get; set; }

        public TrainDto? Train { get; set; }
        public SeatTariffDto? SeatTariff { get; set; }
    }
}
