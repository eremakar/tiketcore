
namespace Ticketing.Models.Dtos
{
    /// <summary>
    /// Оплата билета
    /// </summary>
    public partial class TicketPaymentDto
    {
        public long Id { get; set; }
        public DateTime Time { get; set; }
        public double Price { get; set; }
        public int State { get; set; }
        public long? TicketId { get; set; }
        public int? UserId { get; set; }

        public TicketDto? Ticket { get; set; }
        public UserDto? User { get; set; }
    }
}
