
namespace Ticketing.Models.Dtos
{
    /// <summary>
    /// Перевозчик
    /// </summary>
    public partial class CarrierDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? BIN { get; set; }
        public string? Description { get; set; }
        public string? Filial { get; set; }
        public object? Logo { get; set; }
    }
}
