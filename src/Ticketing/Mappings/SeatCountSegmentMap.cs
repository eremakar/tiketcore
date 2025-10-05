using Data.Mapping;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Models.Dtos;
using Newtonsoft.Json;
using Data.Repository.Helpers;

namespace Ticketing.Mappings
{
    /// <summary>
    /// Сегмент по количеству мест
    /// </summary>
    public partial class SeatCountSegmentMap : MapBase2<SeatCountSegment, SeatCountSegmentDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public SeatCountSegmentMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override SeatCountSegmentDto MapCore(SeatCountSegment source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new SeatCountSegmentDto();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.SeatCount = source.SeatCount;
                result.FreeCount = source.FreeCount;
                result.Price = source.Price;
                result.Tickets = source.Tickets;
                result.FromId = source.FromId;
                result.ToId = source.ToId;
                result.TrainId = source.TrainId;
                result.WagonId = source.WagonId;
                result.TrainScheduleId = source.TrainScheduleId;
            }
            if (options.MapObjects)
            {
                result.From = mapContext.RouteStationMap.Map(source.From, options);
                result.To = mapContext.RouteStationMap.Map(source.To, options);
                result.Train = mapContext.TrainMap.Map(source.Train, options);
                result.Wagon = mapContext.TrainWagonMap.Map(source.Wagon, options);
                result.TrainSchedule = mapContext.TrainScheduleMap.Map(source.TrainSchedule, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override SeatCountSegment ReverseMapCore(SeatCountSegmentDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new SeatCountSegment();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.SeatCount = source.SeatCount;
                result.FreeCount = source.FreeCount;
                result.Price = source.Price;
                if (source.Tickets != null)
                    result.Tickets = JsonConvert.SerializeObject(source.Tickets);
                result.FromId = source.FromId;
                result.ToId = source.ToId;
                result.TrainId = source.TrainId;
                result.WagonId = source.WagonId;
                result.TrainScheduleId = source.TrainScheduleId;
            }
            if (options.MapObjects)
            {
                if (source.FromId == null)
                    result.From = mapContext.RouteStationMap.ReverseMap(source.From, options);
                if (source.ToId == null)
                    result.To = mapContext.RouteStationMap.ReverseMap(source.To, options);
                if (source.TrainId == null)
                    result.Train = mapContext.TrainMap.ReverseMap(source.Train, options);
                if (source.WagonId == null)
                    result.Wagon = mapContext.TrainWagonMap.ReverseMap(source.Wagon, options);
                if (source.TrainScheduleId == null)
                    result.TrainSchedule = mapContext.TrainScheduleMap.ReverseMap(source.TrainSchedule, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override void MapCore(SeatCountSegment source, SeatCountSegment destination, MapOptions options = null)
        {
            if (source == null || destination == null)
                return;

            options = options ?? new MapOptions();

            destination.Id = source.Id;
            if (options.MapProperties)
            {
                destination.SeatCount = source.SeatCount;
                destination.FreeCount = source.FreeCount;
                destination.Price = source.Price;
                destination.Tickets = JsonHelper.NormalizeSafe(source.Tickets);
                destination.FromId = source.FromId;
                destination.ToId = source.ToId;
                destination.TrainId = source.TrainId;
                destination.WagonId = source.WagonId;
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
