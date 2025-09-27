
namespace Ticketing.Models.Dtos
{
    /// <summary>
    /// План состава поезда
    /// </summary>
    public partial class TrainWagonsPlanDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public long? TrainId { get; set; }

        public TrainDto? Train { get; set; }

        public List<TrainWagonsPlanWagonDto>? Wagons { get; set; }
    }
}
