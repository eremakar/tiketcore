using Data.Repository;

namespace Ticketing.Data.TicketDb.Entities.Dictionaries
{
    /// <summary>
    /// Тип поезда
    /// </summary>
    public partial class TrainType : IEntityKey<long>
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
    }
}
