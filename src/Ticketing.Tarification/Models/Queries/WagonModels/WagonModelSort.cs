using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;

namespace Ticketing.Tarifications.Models.Queries.WagonModels
{
    /// <summary>
    /// Вагон (тип)
    /// </summary>
    public partial class WagonModelSort : SortBase<WagonModel>
    {
        public SortOperand? Id { get; set; }
        public SortOperand? Name { get; set; }
        public SortOperand? SeatCount { get; set; }
        public SortOperand? PictureS3 { get; set; }
        public SortOperand? Class { get; set; }
        public SortOperand? TypeId { get; set; }
    }
}
