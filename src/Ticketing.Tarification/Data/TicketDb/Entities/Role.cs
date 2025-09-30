using Data.Repository;

namespace Ticketing.Tarifications.Data.TicketDb.Entities
{
    public partial class Role : IEntityKey<int>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
    }
}
