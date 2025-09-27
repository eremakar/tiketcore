
namespace Ticketing.Models.Dtos
{
    /// <summary>
    /// Тип вагона
    /// </summary>
    public partial class WagonTypeDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? ShortName { get; set; }
        public string? Code { get; set; }
    }
}
