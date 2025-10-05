using Data.Mapping;
using Data.Repository.Helpers;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Models.Dtos;

namespace Ticketing.Mappings
{
    /// <summary>
    /// Станция маршрута
    /// </summary>
    public partial class RouteStationMap : MapBase2<RouteStation, RouteStationDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public RouteStationMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override RouteStationDto MapCore(RouteStation source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new RouteStationDto();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Order = source.Order;
                result.Arrival = source.Arrival;
                result.Stop = source.Stop;
                result.Departure = source.Departure;
                result.Distance = source.Distance;
                result.StationId = source.StationId;
                result.RouteId = source.RouteId;
            }
            if (options.MapObjects)
            {
                result.Station = mapContext.StationMap.Map(source.Station, options);
                result.Route = mapContext.RouteMap.Map(source.Route, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override RouteStation ReverseMapCore(RouteStationDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new RouteStation();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Order = source.Order;
                result.Arrival = source.Arrival != null ? source.Arrival.Value.ToUtc() : null;
                result.Stop = source.Stop != null ? source.Stop.Value.ToUtc() : null;
                result.Departure = source.Departure != null ? source.Departure.Value.ToUtc() : null;
                result.Distance = source.Distance;
                result.StationId = source.StationId;
                result.RouteId = source.RouteId;
            }
            if (options.MapObjects)
            {
                if (source.StationId == null)
                    result.Station = mapContext.StationMap.ReverseMap(source.Station, options);
                if (source.RouteId == null)
                    result.Route = mapContext.RouteMap.ReverseMap(source.Route, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override void MapCore(RouteStation source, RouteStation destination, MapOptions options = null)
        {
            if (source == null || destination == null)
                return;

            options = options ?? new MapOptions();

            destination.Id = source.Id;
            if (options.MapProperties)
            {
                destination.Order = source.Order;
                destination.Arrival = source.Arrival;
                destination.Stop = source.Stop;
                destination.Departure = source.Departure;
                destination.Distance = source.Distance;
                destination.StationId = source.StationId;
                destination.RouteId = source.RouteId;
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
