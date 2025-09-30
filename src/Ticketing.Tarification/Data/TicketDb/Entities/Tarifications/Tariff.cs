using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;

namespace Ticketing.Tarifications.Data.TicketDb.Entities.Tarifications
{
    /// <summary>
    /// Тариф
    /// </summary>
    public partial class Tariff : IEntityKey<long>
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public double IndexCoefficient { get; set; }
        public double VAT { get; set; }
        public long? BaseFareId { get; set; }
        public long? TrainCategoryId { get; set; }
        public long? WagonId { get; set; }

        public BaseFare? BaseFare { get; set; }
        public TrainCategory? TrainCategory { get; set; }
        public Wagon? Wagon { get; set; }
    }
}
