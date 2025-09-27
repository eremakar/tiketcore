using Data.Mapping;
using Data.Repository.Helpers;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Models.Dtos;

namespace Ticketing.Mappings
{
    /// <summary>
    /// График времени на станции
    /// </summary>
    public partial class TimeScheduleMap : MapBase2<TimeSchedule, TimeScheduleDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public TimeScheduleMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override TimeScheduleDto MapCore(TimeSchedule source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new TimeScheduleDto();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Arrival = source.Arrival;
                result.Stop = source.Stop;
                result.Departure = source.Departure;
                result.TrainId = source.TrainId;
                result.RouteStationId = source.RouteStationId;
            }
            if (options.MapObjects)
            {
                result.Train = mapContext.TrainMap.Map(source.Train, options);
                result.RouteStation = mapContext.RouteStationMap.Map(source.RouteStation, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override TimeSchedule ReverseMapCore(TimeScheduleDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new TimeSchedule();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Arrival = source.Arrival != null ? source.Arrival.Value.ToUtc() : null;
                result.Stop = source.Stop;
                result.Departure = source.Departure != null ? source.Departure.Value.ToUtc() : null;
                result.TrainId = source.TrainId;
                result.RouteStationId = source.RouteStationId;
            }
            if (options.MapObjects)
            {
                result.Train = mapContext.TrainMap.ReverseMap(source.Train, options);
                result.RouteStation = mapContext.RouteStationMap.ReverseMap(source.RouteStation, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override void MapCore(TimeSchedule source, TimeSchedule destination, MapOptions options = null)
        {
            if (source == null || destination == null)
                return;

            options = options ?? new MapOptions();

            destination.Id = source.Id;
            if (options.MapProperties)
            {
                destination.Arrival = source.Arrival;
                destination.Stop = source.Stop;
                destination.Departure = source.Departure;
                destination.TrainId = source.TrainId;
                destination.RouteStationId = source.RouteStationId;
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
