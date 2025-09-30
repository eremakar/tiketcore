using Data.Mapping;
using Data.Repository.Helpers;
using Ticketing.Tarifications.Data.TicketDb.Entities;
using Ticketing.Tarifications.Models.Dtos;

namespace Ticketing.Tarifications.Mappings
{
    /// <summary>
    /// Бронирование количества мест
    /// </summary>
    public partial class SeatCountReservationMap : MapBase2<SeatCountReservation, SeatCountReservationDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public SeatCountReservationMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override SeatCountReservationDto MapCore(SeatCountReservation source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new SeatCountReservationDto();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Number = source.Number;
                result.DateTime = source.DateTime;
                result.Price = source.Price;
                result.Total = source.Total;
                result.SeatCount = source.SeatCount;
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

        public override SeatCountReservation ReverseMapCore(SeatCountReservationDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new SeatCountReservation();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Number = source.Number;
                result.DateTime = source.DateTime.ToUtc();
                result.Price = source.Price;
                result.Total = source.Total;
                result.SeatCount = source.SeatCount;
                result.FromId = source.FromId;
                result.ToId = source.ToId;
                result.TrainId = source.TrainId;
                result.WagonId = source.WagonId;
                result.TrainScheduleId = source.TrainScheduleId;
            }
            if (options.MapObjects)
            {
                result.From = mapContext.RouteStationMap.ReverseMap(source.From, options);
                result.To = mapContext.RouteStationMap.ReverseMap(source.To, options);
                result.Train = mapContext.TrainMap.ReverseMap(source.Train, options);
                result.Wagon = mapContext.TrainWagonMap.ReverseMap(source.Wagon, options);
                result.TrainSchedule = mapContext.TrainScheduleMap.ReverseMap(source.TrainSchedule, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override void MapCore(SeatCountReservation source, SeatCountReservation destination, MapOptions options = null)
        {
            if (source == null || destination == null)
                return;

            options = options ?? new MapOptions();

            destination.Id = source.Id;
            if (options.MapProperties)
            {
                destination.Number = source.Number;
                destination.DateTime = source.DateTime;
                destination.Price = source.Price;
                destination.Total = source.Total;
                destination.SeatCount = source.SeatCount;
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
