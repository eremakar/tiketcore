using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities.Tarifications;

namespace Ticketing.Tarifications.Models.Queries.Tarifications.Seasons
{
    /// <summary>
    /// Сезонность
    /// </summary>
    public partial class SeasonSort : SortBase<Season>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? Name { get; set; }
        public SortOperand? Code { get; set; }
        /// <summary>
        /// Тарифный коэффициент
        /// </summary>
        public SortOperand? TarifCoefficient { get; set; }
    }
}
