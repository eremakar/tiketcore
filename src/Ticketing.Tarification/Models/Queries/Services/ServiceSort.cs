using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;

namespace Ticketing.Tarifications.Models.Queries.Services
{
    /// <summary>
    /// Сервис/услуга
    /// </summary>
    public partial class ServiceSort : SortBase<Service>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? Name { get; set; }
        public SortOperand? Code { get; set; }
    }
}
