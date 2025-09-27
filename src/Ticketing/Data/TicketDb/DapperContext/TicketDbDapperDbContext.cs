using Data.Repository.Dapper;
using Microsoft.Extensions.Configuration;

namespace Ticketing.Data.TicketDb.DapperContext
{
    public partial class TicketDbDapperDbContext : DapperDbContext
    {
        public TicketDbDapperDbContext(IConfiguration configuration)
            : base(configuration)
        {
        }
    }
}
