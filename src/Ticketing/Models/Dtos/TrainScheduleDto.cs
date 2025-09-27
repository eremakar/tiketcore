
namespace Ticketing.Models.Dtos
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

        public TrainDto? Train { get; set; }
    }
}
