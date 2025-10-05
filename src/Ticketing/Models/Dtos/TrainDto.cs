using Ticketing.Models.Dtos.Tarifications;

namespace Ticketing.Models.Dtos
{
    /// <summary>
    /// Поезд
    /// </summary>
    public partial class TrainDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        /// <summary>
        /// Тип поезда, например Тальго
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// Зональность: пригородный, общий и т.п.
        /// </summary>
        public int ZoneType { get; set; }
        public long? FromId { get; set; }
        public long? ToId { get; set; }
        public long? RouteId { get; set; }
        public long? PlanId { get; set; }
        public long? CategoryId { get; set; }

        public StationDto? From { get; set; }
        public StationDto? To { get; set; }
        public RouteDto? Route { get; set; }
        public TrainWagonsPlanDto? Plan { get; set; }
        public TrainCategoryDto? Category { get; set; }
    }
}
