using System.Text.Json;
using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketing.Tarifications.Data.TicketDb.Entities.Tarifications
{
    /// <summary>
    /// Элемент тарифа места
    /// </summary>
    public partial class SeatTariffItem : IEntityKey<long>
    {
        public long Id { get; set; }
        [Column(TypeName = "jsonb")]
        public string? CalculationParameters { get; set; }
        public double Distance { get; set; }
        public double Price { get; set; }
        public long? WagonClassId { get; set; }
        public long? SeasonId { get; set; }
        public long? WagonId { get; set; }
        public long? SeatTypeId { get; set; }
        public long? FromId { get; set; }
        public long? ToId { get; set; }
        public long? SeatTariffId { get; set; }

        public WagonClass? WagonClass { get; set; }
        public Season? Season { get; set; }
        public WagonModel? Wagon { get; set; }
        public SeatType? SeatType { get; set; }
        public Station? From { get; set; }
        public Station? To { get; set; }
        public SeatTariff? SeatTariff { get; set; }
    }
}
