using Data.Repository;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketing.Data.TicketDb.Entities.Tarifications
{
    /// <summary>
    /// Тариф
    /// </summary>
    public partial class Tariff : IEntityKey<long>
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public double VAT { get; set; }
        public long? BaseFareId { get; set; }

        public BaseFare? BaseFare { get; set; }

        [InverseProperty("Tariff")]
        public List<TariffTrainCategoryItem>? TrainCategories { get; set; }
        [InverseProperty("Tariff")]
        public List<TariffWagonItem>? Wagons { get; set; }
        [InverseProperty("Tariff")]
        public List<TariffWagonTypeItem>? WagonTypes { get; set; }
    }
}
