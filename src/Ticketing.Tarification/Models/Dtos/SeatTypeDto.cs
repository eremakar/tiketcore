
namespace Ticketing.Tarifications.Models.Dtos
{
    /// <summary>
    /// Тип места
    /// </summary>
    public partial class SeatTypeDto
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
