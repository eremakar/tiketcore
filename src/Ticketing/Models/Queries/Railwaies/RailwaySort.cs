using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.Railwaies
{
    /// <summary>
    /// ЖД дорога
    /// </summary>
    public partial class RailwaySort : SortBase<Railway>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? Name { get; set; }
        public SortOperand? Code { get; set; }
        public SortOperand? ShortCode { get; set; }
        public SortOperand? TimeDifferenceFromAdministration { get; set; }
        public SortOperand? Type { get; set; }
    }
}
