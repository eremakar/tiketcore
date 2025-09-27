using Data.Mapping;
using Data.Repository.Helpers;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Models.Dtos;

namespace Ticketing.Mappings
{
    /// <summary>
    /// Билет
    /// </summary>
    public partial class TicketMap : MapBase2<Ticket, TicketDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public TicketMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override TicketDto MapCore(Ticket source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new TicketDto();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Number = source.Number;
                result.Date = source.Date;
                result.IsSeat = source.IsSeat;
                result.Price = source.Price;
                result.State = source.State;
                result.Type = source.Type;
                result.Total = source.Total;
                result.FromId = source.FromId;
                result.ToId = source.ToId;
                result.TrainId = source.TrainId;
                result.WagonId = source.WagonId;
                result.SeatId = source.SeatId;
                result.TrainScheduleId = source.TrainScheduleId;
            }
            if (options.MapObjects)
            {
                result.From = mapContext.RouteStationMap.Map(source.From, options);
                result.To = mapContext.RouteStationMap.Map(source.To, options);
                result.Train = mapContext.TrainMap.Map(source.Train, options);
                result.Wagon = mapContext.TrainWagonMap.Map(source.Wagon, options);
                result.Seat = mapContext.SeatMap.Map(source.Seat, options);
                result.TrainSchedule = mapContext.TrainScheduleMap.Map(source.TrainSchedule, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override Ticket ReverseMapCore(TicketDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new Ticket();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Number = source.Number;
                result.Date = source.Date.ToUtc();
                result.IsSeat = source.IsSeat;
                result.Price = source.Price;
                result.State = source.State;
                result.Type = source.Type;
                result.Total = source.Total;
                result.FromId = source.FromId;
                result.ToId = source.ToId;
                result.TrainId = source.TrainId;
                result.WagonId = source.WagonId;
                result.SeatId = source.SeatId;
                result.TrainScheduleId = source.TrainScheduleId;
            }
            if (options.MapObjects)
            {
                result.From = mapContext.RouteStationMap.ReverseMap(source.From, options);
                result.To = mapContext.RouteStationMap.ReverseMap(source.To, options);
                result.Train = mapContext.TrainMap.ReverseMap(source.Train, options);
                result.Wagon = mapContext.TrainWagonMap.ReverseMap(source.Wagon, options);
                result.Seat = mapContext.SeatMap.ReverseMap(source.Seat, options);
                result.TrainSchedule = mapContext.TrainScheduleMap.ReverseMap(source.TrainSchedule, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override void MapCore(Ticket source, Ticket destination, MapOptions options = null)
        {
            if (source == null || destination == null)
                return;

            options = options ?? new MapOptions();

            destination.Id = source.Id;
            if (options.MapProperties)
            {
                destination.Number = source.Number;
                destination.Date = source.Date;
                destination.IsSeat = source.IsSeat;
                destination.Price = source.Price;
                destination.State = source.State;
                destination.Type = source.Type;
                destination.Total = source.Total;
                destination.FromId = source.FromId;
                destination.ToId = source.ToId;
                destination.TrainId = source.TrainId;
                destination.WagonId = source.WagonId;
                destination.SeatId = source.SeatId;
                destination.TrainScheduleId = source.TrainScheduleId;
            }
            if (options.MapObjects)
            {
            }
            if (options.MapCollections)
            {
            }

        }
    }
}
