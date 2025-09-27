
namespace Ticketing.Models.Dtos
{
    /// <summary>
    /// Статус билета
    /// </summary>
    public partial class TicketStateDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
    }
}
