
namespace Ticketing.Tarifications.Models.Dtos.Tarifications
{
    /// <summary>
    /// Категория поезда
    /// </summary>
    public partial class TrainCategoryDto
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
