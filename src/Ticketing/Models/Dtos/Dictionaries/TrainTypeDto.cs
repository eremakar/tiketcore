
namespace Ticketing.Models.Dtos.Dictionaries
{
    /// <summary>
    /// Тип поезда
    /// </summary>
    public partial class TrainTypeDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
    }
}
