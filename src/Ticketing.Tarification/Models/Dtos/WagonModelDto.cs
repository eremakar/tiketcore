
namespace Ticketing.Tarifications.Models.Dtos
{
    /// <summary>
    /// Вагон (тип)
    /// </summary>
    public partial class WagonModelDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public int SeatCount { get; set; }
        public object? PictureS3 { get; set; }
        public string? Class { get; set; }
        public long? TypeId { get; set; }

        public WagonTypeDto? Type { get; set; }

        public List<SeatDto>? Seats { get; set; }
    }
}
