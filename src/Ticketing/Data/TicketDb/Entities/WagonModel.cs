using System.Text.Json;
using Data.Repository;
using Ticketing.Data.TicketDb.Entities.Tarifications;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketing.Data.TicketDb.Entities
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
        public long? ClassId { get; set; }
        public long? TypeId { get; set; }

        public WagonClass? Class { get; set; }
        public WagonType? Type { get; set; }

        [InverseProperty("Wagon")]
        public List<WagonModelFeature>? Features { get; set; }
        [InverseProperty("Wagon")]
        public List<Seat>? Seats { get; set; }
    }
}
