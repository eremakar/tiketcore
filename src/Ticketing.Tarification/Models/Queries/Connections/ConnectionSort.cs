using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;

namespace Ticketing.Tarifications.Models.Queries.Connections
{
    /// <summary>
    /// Соединение 2х станций
    /// </summary>
    public partial class ConnectionSort : SortBase<Connection>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? Name { get; set; }
        public SortOperand? DistanceKm { get; set; }
        public SortOperand? FromId { get; set; }
        public SortOperand? ToId { get; set; }
    }
}
