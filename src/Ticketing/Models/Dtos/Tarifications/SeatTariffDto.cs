using Ticketing.Models.Dtos;

namespace Ticketing.Models.Dtos.Tarifications
{
    /// <summary>
    /// Тариф места в вагоне
    /// </summary>
    public partial class SeatTariffDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public double Price { get; set; }
        public long? BaseFareId { get; set; }
        public long? TrainId { get; set; }
        public long? TrainCategoryId { get; set; }
        public long? WagonClassId { get; set; }
        public long? SeasonId { get; set; }
        public long? SeatTypeId { get; set; }
        /// <summary>
        /// соединение станций
        /// </summary>
        public long? ConnectionId { get; set; }

        public BaseFareDto? BaseFare { get; set; }
        public TrainDto? Train { get; set; }
        public TrainCategoryDto? TrainCategory { get; set; }
        public WagonClassDto? WagonClass { get; set; }
        public SeasonDto? Season { get; set; }
        public SeatTypeDto? SeatType { get; set; }
        /// <summary>
        /// соединение станций
        /// </summary>
        public ConnectionDto? Connection { get; set; }
    }
}
