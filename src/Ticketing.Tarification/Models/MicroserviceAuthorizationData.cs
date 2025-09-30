using Api.AspNetCore.Models.Secure;
using Data.Repository;

namespace Ticketing.Tarifications.Models
{
    public partial class MicroserviceAuthorizationData : AuthorizationData
    {
        public bool IsAdministrator { get { return Roles.Any(_ => _ == "SuperAdministrator" || _ == "Administrator"); } }
        public bool? IsApproved { get; set; }
        public int? FilialId { get; set; }
    }
}
