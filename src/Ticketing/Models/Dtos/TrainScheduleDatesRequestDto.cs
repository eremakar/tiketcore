using System.ComponentModel.DataAnnotations;

namespace Ticketing.Models.Dtos
{
    /// <summary>
    /// Request DTO for creating train schedules by dates
    /// </summary>
    public class TrainScheduleDatesRequestDto
    {
        /// <summary>
        /// Train ID
        /// </summary>
        [Required]
        public long TrainId { get; set; }

        /// <summary>
        /// List of dates to create schedules for
        /// </summary>
        [Required]
        public List<DateTime> Dates { get; set; } = new List<DateTime>();
    }
}
