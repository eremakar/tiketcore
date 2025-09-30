using Data.Repository;
using Ticketing.Tarifications.Data.TicketDb.Entities;

namespace Ticketing.Tarifications.Models.Queries.Trains
{
    public partial class TrainQuery : QueryBase<Train, TrainFilter, TrainSort>
    {
    }
}
