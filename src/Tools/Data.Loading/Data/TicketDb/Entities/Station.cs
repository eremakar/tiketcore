using System.Text.Json;
using Data.Repository;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketing.Data.TicketDb.Entities
{
    /// <summary>
    /// Станция
    /// </summary>
    public partial class Station : IEntityKey<long>
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? ShortName { get; set; }
        public string? ShortNameLatin { get; set; }
        [Column(TypeName = "jsonb")]
        public string? Depots { get; set; }
        public bool IsCity { get; set; }
        public string? CityCode { get; set; }
        public bool IsSalePoint { get; set; }
    }
}
