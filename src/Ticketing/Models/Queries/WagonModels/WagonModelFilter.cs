using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.WagonModels
{
    /// <summary>
    /// Вагон (тип)
    /// </summary>
    public partial class WagonModelFilter : FilterBase<WagonModel>
    {
        public FilterOperand<long>? Id { get; set; }
        public FilterOperand<string>? Name { get; set; }
        public FilterOperand<int>? SeatCount { get; set; }
        public FilterOperand<object>? PictureS3 { get; set; }
        /// <summary>
        /// Наличие подъемного механизма
        /// </summary>
        public FilterOperand<bool>? HasLiftingMechanism { get; set; }
        /// <summary>
        /// Завод изготовитель
        /// </summary>
        public FilterOperand<string>? ManufacturerName { get; set; }
        public FilterOperand<long?>? ClassId { get; set; }
        public FilterOperand<long?>? TypeId { get; set; }
    }
}
