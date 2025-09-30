using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;

namespace Ticketing.Tarifications.Models.Queries.UserRoles
{
    public partial class UserRoleFilter : FilterBase<UserRole>
    {
        public FilterOperand<int>? Id { get; set; }
        public FilterOperand<int?>? UserId { get; set; }
        public FilterOperand<int?>? RoleId { get; set; }
    }
}
