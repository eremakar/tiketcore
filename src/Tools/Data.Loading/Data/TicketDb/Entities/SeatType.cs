using Data.Repository;

namespace Ticketing.Data.TicketDb.Entities
{
    /// <summary>
    /// Тип места
    /// </summary>
    public partial class SeatType : IEntityKey<long>
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        /// <summary>
        /// Тарифный коэффициент
        /// </summary>
        public double TarifCoefficient { get; set; }
    }
}
