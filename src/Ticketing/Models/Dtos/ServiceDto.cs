
namespace Ticketing.Models.Dtos
{
    /// <summary>
    /// Сервис/услуга
    /// </summary>
    public partial class ServiceDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
    }
}
