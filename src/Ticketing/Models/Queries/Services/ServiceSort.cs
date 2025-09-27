using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.Services
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
