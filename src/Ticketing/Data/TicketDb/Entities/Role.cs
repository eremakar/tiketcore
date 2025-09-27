using Data.Repository;

namespace Ticketing.Data.TicketDb.Entities
{
    public partial class Role : IEntityKey<int>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
    }
}
