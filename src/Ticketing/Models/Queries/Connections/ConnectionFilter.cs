using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.Connections
{
    /// <summary>
    /// Соединение 2х станций
    /// </summary>
    public partial class ConnectionFilter : FilterBase<Connection>
    {
        public FilterOperand<long>? Id { get; set; }
        public FilterOperand<string>? Name { get; set; }
        public FilterOperand<double>? DistanceKm { get; set; }
        public FilterOperand<long?>? FromId { get; set; }
        public FilterOperand<long?>? ToId { get; set; }
    }
}
