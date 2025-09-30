using Data.Repository;

namespace Ticketing.Tarifications.Data.TicketDb.Entities
{
    /// <summary>
    /// Оплата билета
    /// </summary>
    public partial class TicketPayment : IEntityKey<long>
    {
        public long Id { get; set; }
        public DateTime Time { get; set; }
        public double Price { get; set; }
        public int State { get; set; }
        public long? TicketId { get; set; }
        public int? UserId { get; set; }

        public Ticket? Ticket { get; set; }
        public User? User { get; set; }
    }
}
