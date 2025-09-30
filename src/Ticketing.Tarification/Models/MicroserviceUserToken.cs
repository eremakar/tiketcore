using Api.AspNetCore.Models.Secure;

namespace Ticketing.Tarifications.Models
{
    public partial class MicroserviceUserToken : JwtToken
    {
        public bool? IsApproved { get; set; }
        public int? FilialId { get; set; }
    }
}
