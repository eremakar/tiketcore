
namespace Ticketing.Tarifications.Models.Dtos.Tarifications
{
    /// <summary>
    /// Базовая ставка
    /// </summary>
    public partial class BaseFareDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public double Price { get; set; }
    }
}
