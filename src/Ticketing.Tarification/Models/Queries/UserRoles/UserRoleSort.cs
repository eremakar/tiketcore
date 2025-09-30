using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;

namespace Ticketing.Tarifications.Models.Queries.UserRoles
{
    public partial class UserRoleSort : SortBase<UserRole>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? UserId { get; set; }
        public SortOperand? RoleId { get; set; }
    }
}
