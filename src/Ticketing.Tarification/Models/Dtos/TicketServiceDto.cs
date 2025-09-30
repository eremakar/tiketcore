
namespace Ticketing.Tarifications.Models.Dtos
{
    /// <summary>
    /// Услуги билета
    /// </summary>
    public partial class TicketServiceDto
    {
        public long Id { get; set; }
        public int State { get; set; }
        public long? TicketId { get; set; }
        public long? ServiceId { get; set; }

        public TicketDto? Ticket { get; set; }
        public ServiceDto? Service { get; set; }
    }
}
