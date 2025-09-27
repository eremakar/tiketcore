using Data.Repository;

namespace Ticketing.Data.TicketDb.Entities
{
    public partial class UserRole : IEntityKey<int>
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? RoleId { get; set; }

        public User? User { get; set; }
        public Role? Role { get; set; }
    }
}
