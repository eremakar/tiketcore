using System.Text.Json;
using Data.Repository;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketing.Tarifications.Data.TicketDb.Entities
{
    /// <summary>
    /// Вагон (тип)
    /// </summary>
    public partial class WagonModel : IEntityKey<long>
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public int SeatCount { get; set; }
        [Column(TypeName = "jsonb")]
        public string? PictureS3 { get; set; }
        public string? Class { get; set; }
        public long? TypeId { get; set; }

        public WagonType? Type { get; set; }

        [InverseProperty("Wagon")]
        public List<Seat>? Seats { get; set; }
    }
}
