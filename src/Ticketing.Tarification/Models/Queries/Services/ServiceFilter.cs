using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;

namespace Ticketing.Tarifications.Models.Queries.Services
{
    /// <summary>
    /// Сервис/услуга
    /// </summary>
    public partial class ServiceFilter : FilterBase<Service>
    {
        public FilterOperand<long>? Id { get; set; }
        public FilterOperand<string>? Name { get; set; }
        public FilterOperand<string>? Code { get; set; }
    }
}
