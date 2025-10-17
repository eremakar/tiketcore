using Data.Repository;

namespace Ticketing.Data.TicketDb.Entities
{
    /// <summary>
    /// Особенности модели вагона
    /// </summary>
    public partial class WagonModelFeature : IEntityKey<long>
    {
        public long Id { get; set; }
        public long? WagonId { get; set; }
        public int? FeatureId { get; set; }

        public WagonModel? Wagon { get; set; }
        public WagonFeature? Feature { get; set; }
    }
}
