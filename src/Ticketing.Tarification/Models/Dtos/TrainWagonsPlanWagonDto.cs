
namespace Ticketing.Tarifications.Models.Dtos
{
    /// <summary>
    /// Вагон в плане состава
    /// </summary>
    public partial class TrainWagonsPlanWagonDto
    {
        public long Id { get; set; }
        public string? Number { get; set; }
        public long? PlanId { get; set; }
        public long? WagonId { get; set; }

        public TrainWagonsPlanDto? Plan { get; set; }
        public WagonModelDto? Wagon { get; set; }
    }
}
