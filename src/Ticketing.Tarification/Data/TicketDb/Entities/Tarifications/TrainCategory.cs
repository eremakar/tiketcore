using Data.Repository;

namespace Ticketing.Tarifications.Data.TicketDb.Entities.Tarifications
{
    /// <summary>
    /// Категория поезда
    /// </summary>
    public partial class TrainCategory : IEntityKey<long>
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
