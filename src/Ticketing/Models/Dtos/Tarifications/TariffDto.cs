
namespace Ticketing.Models.Dtos.Tarifications
{
    /// <summary>
    /// Тариф
    /// </summary>
    public partial class TariffDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public double VAT { get; set; }
        public long? BaseFareId { get; set; }

        public BaseFareDto? BaseFare { get; set; }

        public List<TariffTrainCategoryItemDto>? TrainCategories { get; set; }
        public List<TariffWagonItemDto>? Wagons { get; set; }
        public List<TariffWagonTypeItemDto>? WagonTypes { get; set; }
    }
}
