using System.Text.Json;
using Data.Repository;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketing.Data.TicketDb.Entities
{
    /// <summary>
    /// Перевозчик
    /// </summary>
    public partial class Carrier : IEntityKey<long>
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? BIN { get; set; }
        public string? Description { get; set; }
        public string? Filial { get; set; }
        [Column(TypeName = "jsonb")]
        public string? Logo { get; set; }
    }
}
