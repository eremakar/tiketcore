using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.WagonModels
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
        public SortOperand? ClassId { get; set; }
        public SortOperand? TypeId { get; set; }
    }
}
