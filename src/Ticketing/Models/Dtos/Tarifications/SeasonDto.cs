
namespace Ticketing.Models.Dtos.Tarifications
{
    /// <summary>
    /// Сезонность
    /// </summary>
    public partial class SeasonDto
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
