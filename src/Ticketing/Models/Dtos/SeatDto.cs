
namespace Ticketing.Models.Dtos
{
    /// <summary>
    /// Место в вагоне
    /// </summary>
    public partial class SeatDto
    {
        public long Id { get; set; }
        public string? Number { get; set; }
        public int Class { get; set; }
        public long? WagonId { get; set; }
        /// <summary>
        /// Тип места: верхний/боковой/нижний
        /// </summary>
        public long? TypeId { get; set; }

        public WagonModelDto? Wagon { get; set; }
        /// <summary>
        /// Тип места: верхний/боковой/нижний
        /// </summary>
        public SeatTypeDto? Type { get; set; }
    }
}
