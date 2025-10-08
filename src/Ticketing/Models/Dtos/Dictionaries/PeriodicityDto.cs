
namespace Ticketing.Models.Dtos.Dictionaries
{
    /// <summary>
    /// Периодичность
    /// </summary>
    public partial class PeriodicityDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
    }
}
