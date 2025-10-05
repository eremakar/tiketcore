using Data.Mapping;
using Data.Repository.Helpers;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Models.Dtos;

namespace Ticketing.Mappings
{
    /// <summary>
    /// Бронирование места
    /// </summary>
    public partial class SeatReservationMap : MapBase2<SeatReservation, SeatReservationDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public SeatReservationMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override SeatReservationDto MapCore(SeatReservation source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new SeatReservationDto();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Number = source.Number;
                result.Date = source.Date;
                result.Price = source.Price;
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
                result.Segments = mapContext.SeatSegmentMap.Map(source.Segments, options);
            }

            return result;
        }

        public override SeatReservation ReverseMapCore(SeatReservationDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new SeatReservation();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Number = source.Number;
                result.Date = source.Date.ToUtc();
                result.Price = source.Price;
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
                if (source.FromId == null)
                    result.From = mapContext.RouteStationMap.ReverseMap(source.From, options);
                if (source.ToId == null)
                    result.To = mapContext.RouteStationMap.ReverseMap(source.To, options);
                if (source.TrainId == null)
                    result.Train = mapContext.TrainMap.ReverseMap(source.Train, options);
                if (source.WagonId == null)
                    result.Wagon = mapContext.TrainWagonMap.ReverseMap(source.Wagon, options);
                if (source.SeatId == null)
                    result.Seat = mapContext.SeatMap.ReverseMap(source.Seat, options);
                if (source.TrainScheduleId == null)
                    result.TrainSchedule = mapContext.TrainScheduleMap.ReverseMap(source.TrainSchedule, options);
            }
            if (options.MapCollections)
            {
                result.Segments = mapContext.SeatSegmentMap.ReverseMap(source.Segments, options);
            }

            return result;
        }

        public override void MapCore(SeatReservation source, SeatReservation destination, MapOptions options = null)
        {
            if (source == null || destination == null)
                return;

            options = options ?? new MapOptions();

            destination.Id = source.Id;
            if (options.MapProperties)
            {
                destination.Number = source.Number;
                destination.Date = source.Date;
                destination.Price = source.Price;
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
