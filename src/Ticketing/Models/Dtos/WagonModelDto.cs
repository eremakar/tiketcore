using Ticketing.Models.Dtos.Tarifications;

namespace Ticketing.Models.Dtos
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
        /// <summary>
        /// Наличие подъемного механизма
        /// </summary>
        public bool HasLiftingMechanism { get; set; }
        /// <summary>
        /// Завод изготовитель
        /// </summary>
        public string? ManufacturerName { get; set; }
        public long? ClassId { get; set; }
        public long? TypeId { get; set; }

        public WagonClassDto? Class { get; set; }
        public WagonTypeDto? Type { get; set; }

        public List<WagonModelFeatureDto>? Features { get; set; }
        public List<SeatDto>? Seats { get; set; }
    }
}
