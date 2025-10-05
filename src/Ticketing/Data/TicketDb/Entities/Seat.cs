using Data.Repository;

namespace Ticketing.Data.TicketDb.Entities
{
    /// <summary>
    /// Место в вагоне
    /// </summary>
    public partial class Seat : IEntityKey<long>
    {
        public long Id { get; set; }
        public string? Number { get; set; }
        public int Class { get; set; }
        public long? WagonId { get; set; }
        /// <summary>
        /// Тип места: верхний/боковой/нижний
        /// </summary>
        public long? TypeId { get; set; }
        /// <summary>
        /// Назначение места
        /// </summary>
        public long? PurposeId { get; set; }

        public WagonModel? Wagon { get; set; }
        public SeatType? Type { get; set; }
        public SeatPurpose? Purpose { get; set; }
    }
}
