using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.UserRoles
{
    public partial class UserRoleSort : SortBase<UserRole>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? UserId { get; set; }
        public SortOperand? RoleId { get; set; }
    }
}
