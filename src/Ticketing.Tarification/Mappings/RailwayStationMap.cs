using Data.Mapping;
using Ticketing.Tarifications.Data.TicketDb.Entities;
using Ticketing.Tarifications.Models.Dtos;

namespace Ticketing.Tarifications.Mappings
{
    /// <summary>
    /// Станция дороги
    /// </summary>
    public partial class RailwayStationMap : MapBase2<RailwayStation, RailwayStationDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public RailwayStationMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override RailwayStationDto MapCore(RailwayStation source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new RailwayStationDto();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Order = source.Order;
                result.Distance = source.Distance;
                result.StationId = source.StationId;
                result.RailwayId = source.RailwayId;
            }
            if (options.MapObjects)
            {
                result.Station = mapContext.StationMap.Map(source.Station, options);
                result.Railway = mapContext.RailwayMap.Map(source.Railway, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override RailwayStation ReverseMapCore(RailwayStationDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new RailwayStation();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Order = source.Order;
                result.Distance = source.Distance;
                result.StationId = source.StationId;
                result.RailwayId = source.RailwayId;
            }
            if (options.MapObjects)
            {
                result.Station = mapContext.StationMap.ReverseMap(source.Station, options);
                result.Railway = mapContext.RailwayMap.ReverseMap(source.Railway, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override void MapCore(RailwayStation source, RailwayStation destination, MapOptions options = null)
        {
            if (source == null || destination == null)
                return;

            options = options ?? new MapOptions();

            destination.Id = source.Id;
            if (options.MapProperties)
            {
                destination.Order = source.Order;
                destination.Distance = source.Distance;
                destination.StationId = source.StationId;
                destination.RailwayId = source.RailwayId;
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
