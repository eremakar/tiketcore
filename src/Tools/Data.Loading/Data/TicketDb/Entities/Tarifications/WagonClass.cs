using Data.Repository;

namespace Ticketing.Data.TicketDb.Entities.Tarifications
{
    /// <summary>
    /// Класс вагона
    /// </summary>
    public partial class WagonClass : IEntityKey<long>
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
