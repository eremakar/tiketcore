using Data.Repository;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketing.Tarifications.Data.TicketDb.Entities
{
    /// <summary>
    /// ЖД дорога
    /// </summary>
    public partial class Railway : IEntityKey<long>
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? ShortCode { get; set; }
        public int TimeDifferenceFromAdministration { get; set; }
        public string? Type { get; set; }

        [InverseProperty("Railway")]
        public List<RailwayStation>? Stations { get; set; }
    }
}
