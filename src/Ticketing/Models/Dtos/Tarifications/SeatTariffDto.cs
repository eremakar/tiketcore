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
        public long? TrainId { get; set; }
        public long? BaseFareId { get; set; }
        public long? TrainCategoryId { get; set; }

        public TrainDto? Train { get; set; }
        public BaseFareDto? BaseFare { get; set; }
        public TrainCategoryDto? TrainCategory { get; set; }

        public List<SeatTariffItemDto>? Items { get; set; }
    }
}
