using Ticketing.Models.Dtos;

namespace Ticketing.Models.Dtos.Tarifications
{
    /// <summary>
    /// Тариф
    /// </summary>
    public partial class TariffDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public double IndexCoefficient { get; set; }
        public double VAT { get; set; }
        public long? BaseFareId { get; set; }
        public long? TrainCategoryId { get; set; }
        public long? WagonId { get; set; }

        public BaseFareDto? BaseFare { get; set; }
        public TrainCategoryDto? TrainCategory { get; set; }
        public WagonDto? Wagon { get; set; }
    }
}
