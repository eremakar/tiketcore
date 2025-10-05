
namespace Ticketing.Models.Dtos
{
    /// <summary>
    /// Особенности модели вагона
    /// </summary>
    public partial class WagonModelFeatureDto
    {
        public long Id { get; set; }
        public long? WagonId { get; set; }
        public int? FeatureId { get; set; }

        public WagonModelDto? Wagon { get; set; }
        public WagonFeatureDto? Feature { get; set; }
    }
}
