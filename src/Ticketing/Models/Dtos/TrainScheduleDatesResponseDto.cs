namespace Ticketing.Models.Dtos
{
    /// <summary>
    /// Response DTO for train schedule dates creation
    /// </summary>
    public class TrainScheduleDatesResponseDto
    {
        /// <summary>
        /// Number of schedules created
        /// </summary>
        public int SchedulesCreated { get; set; }

        /// <summary>
        /// Number of train wagons created
        /// </summary>
        public int TrainWagonsCreated { get; set; }

        /// <summary>
        /// Number of seats created
        /// </summary>
        public int SeatsCreated { get; set; }

        /// <summary>
        /// Number of seat segments created
        /// </summary>
        public int SeatSegmentsCreated { get; set; }

        /// <summary>
        /// Created schedule IDs
        /// </summary>
        public List<long> ScheduleIds { get; set; } = new List<long>();
    }
}
