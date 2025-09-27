using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.Railwaies
{
    /// <summary>
    /// ЖД дорога
    /// </summary>
    public partial class RailwayFilter : FilterBase<Railway>
    {
        public FilterOperand<long>? Id { get; set; }
        public FilterOperand<string>? Name { get; set; }
        public FilterOperand<string>? Code { get; set; }
        public FilterOperand<string>? ShortCode { get; set; }
        public FilterOperand<int>? TimeDifferenceFromAdministration { get; set; }
        public FilterOperand<string>? Type { get; set; }
    }
}
