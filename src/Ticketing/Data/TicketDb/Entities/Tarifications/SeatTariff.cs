using Data.Repository;
using Ticketing.Data.TicketDb.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketing.Data.TicketDb.Entities.Tarifications
{
    /// <summary>
    /// Тариф места в вагоне
    /// </summary>
    public partial class SeatTariff : IEntityKey<long>
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public long? TrainId { get; set; }
        public long? BaseFareId { get; set; }
        public long? TrainCategoryId { get; set; }
        public long? TariffId { get; set; }

        public Train? Train { get; set; }
        public BaseFare? BaseFare { get; set; }
        public TrainCategory? TrainCategory { get; set; }
        public Tariff? Tariff { get; set; }

        [InverseProperty("SeatTariff")]
        public List<SeatTariffItem>? Items { get; set; }
    }
}
