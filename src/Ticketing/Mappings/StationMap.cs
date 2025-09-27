using Data.Mapping;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Models.Dtos;
using Newtonsoft.Json;
using Data.Repository.Helpers;

namespace Ticketing.Mappings
{
    /// <summary>
    /// Станция
    /// </summary>
    public partial class StationMap : MapBase2<Station, StationDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public StationMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override StationDto MapCore(Station source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new StationDto();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Name = source.Name;
                result.Code = source.Code;
                result.ShortName = source.ShortName;
                result.ShortNameLatin = source.ShortNameLatin;
                result.Depots = source.Depots;
                result.IsCity = source.IsCity;
                result.CityCode = source.CityCode;
                result.IsSalePoint = source.IsSalePoint;
            }
            if (options.MapObjects)
            {
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override Station ReverseMapCore(StationDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new Station();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Name = source.Name;
                result.Code = source.Code;
                result.ShortName = source.ShortName;
                result.ShortNameLatin = source.ShortNameLatin;
                if (source.Depots != null)
                    result.Depots = JsonConvert.SerializeObject(source.Depots);
                result.IsCity = source.IsCity;
                result.CityCode = source.CityCode;
                result.IsSalePoint = source.IsSalePoint;
            }
            if (options.MapObjects)
            {
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override void MapCore(Station source, Station destination, MapOptions options = null)
        {
            if (source == null || destination == null)
                return;

            options = options ?? new MapOptions();

            destination.Id = source.Id;
            if (options.MapProperties)
            {
                destination.Name = source.Name;
                destination.Code = source.Code;
                destination.ShortName = source.ShortName;
                destination.ShortNameLatin = source.ShortNameLatin;
                destination.Depots = JsonHelper.NormalizeSafe(source.Depots);
                destination.IsCity = source.IsCity;
                destination.CityCode = source.CityCode;
                destination.IsSalePoint = source.IsSalePoint;
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
