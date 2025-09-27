using Data.Repository;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Models.Queries.Trains
{
    public partial class TrainQuery : QueryBase<Train, TrainFilter, TrainSort>
    {
    }
}
