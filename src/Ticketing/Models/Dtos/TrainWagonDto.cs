
namespace Ticketing.Models.Dtos
{
    /// <summary>
    /// Вагон состава поезда
    /// </summary>
    public partial class TrainWagonDto
    {
        public long Id { get; set; }
        public string? Number { get; set; }
        public long? TrainScheduleId { get; set; }
        public long? WagonId { get; set; }
        public long? CarrierId { get; set; }

        public TrainScheduleDto? TrainSchedule { get; set; }
        public WagonModelDto? Wagon { get; set; }
        public CarrierDto? Carrier { get; set; }
    }
}
