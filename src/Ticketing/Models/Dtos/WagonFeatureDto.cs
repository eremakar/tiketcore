
namespace Ticketing.Models.Dtos
{
    /// <summary>
    /// Особенности вагона
    /// </summary>
    public partial class WagonFeatureDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
    }
}
