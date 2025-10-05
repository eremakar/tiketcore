
namespace Ticketing.Models.Dtos
{
    /// <summary>
    /// Назначение места
    /// </summary>
    public partial class SeatPurposeDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
    }
}
