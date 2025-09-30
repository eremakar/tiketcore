using Data.Mapping;
using Ticketing.Tarifications.Data.TicketDb.Entities;
using Ticketing.Tarifications.Models.Dtos;

namespace Ticketing.Tarifications.Mappings
{
    /// <summary>
    /// ЖД дорога
    /// </summary>
    public partial class RailwayMap : MapBase2<Railway, RailwayDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public RailwayMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override RailwayDto MapCore(Railway source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new RailwayDto();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Name = source.Name;
                result.Code = source.Code;
                result.ShortCode = source.ShortCode;
                result.TimeDifferenceFromAdministration = source.TimeDifferenceFromAdministration;
                result.Type = source.Type;
            }
            if (options.MapObjects)
            {
            }
            if (options.MapCollections)
            {
                result.Stations = mapContext.RailwayStationMap.Map(source.Stations, options);
            }

            return result;
        }

        public override Railway ReverseMapCore(RailwayDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new Railway();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Name = source.Name;
                result.Code = source.Code;
                result.ShortCode = source.ShortCode;
                result.TimeDifferenceFromAdministration = source.TimeDifferenceFromAdministration;
                result.Type = source.Type;
            }
            if (options.MapObjects)
            {
            }
            if (options.MapCollections)
            {
                result.Stations = mapContext.RailwayStationMap.ReverseMap(source.Stations, options);
            }

            return result;
        }

        public override void MapCore(Railway source, Railway destination, MapOptions options = null)
        {
            if (source == null || destination == null)
                return;

            options = options ?? new MapOptions();

            destination.Id = source.Id;
            if (options.MapProperties)
            {
                destination.Name = source.Name;
                destination.Code = source.Code;
                destination.ShortCode = source.ShortCode;
                destination.TimeDifferenceFromAdministration = source.TimeDifferenceFromAdministration;
                destination.Type = source.Type;
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
