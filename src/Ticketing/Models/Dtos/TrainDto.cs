using Ticketing.Models.Dtos.Dictionaries;
using Ticketing.Models.Dtos.Tarifications;

namespace Ticketing.Models.Dtos
{
    /// <summary>
    /// Поезд по маршруту
    /// </summary>
    public partial class TrainDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        /// <summary>
        /// Зональность: пригородный, общий и т.п.
        /// </summary>
        public int ZoneType { get; set; }
        /// <summary>
        /// 1 - Социально-значимый, 2 - Коммерческий
        /// </summary>
        public int Importance { get; set; }
        /// <summary>
        /// 1 - вагон-ресторан, 2 - вагон-бар
        /// </summary>
        public int Amenities { get; set; }
        /// <summary>
        /// Тип поезда, например Тальго
        /// </summary>
        public long? TypeId { get; set; }
        public long? FromId { get; set; }
        public long? ToId { get; set; }
        public long? RouteId { get; set; }
        public long? PeriodicityId { get; set; }
        public long? PlanId { get; set; }
        public long? CategoryId { get; set; }
        public long? TariffId { get; set; }

        /// <summary>
        /// Тип поезда, например Тальго
        /// </summary>
        public TrainTypeDto? Type { get; set; }
        public StationDto? From { get; set; }
        public StationDto? To { get; set; }
        public RouteDto? Route { get; set; }
        public PeriodicityDto? Periodicity { get; set; }
        public TrainWagonsPlanDto? Plan { get; set; }
        public TrainCategoryDto? Category { get; set; }
        public TariffDto? Tariff { get; set; }
    }
}
