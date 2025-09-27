using Data.Mapping;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Models.Dtos;

namespace Ticketing.Mappings
{
    /// <summary>
    /// Тип вагона
    /// </summary>
    public partial class WagonTypeMap : MapBase2<WagonType, WagonTypeDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public WagonTypeMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override WagonTypeDto MapCore(WagonType source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new WagonTypeDto();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Name = source.Name;
                result.ShortName = source.ShortName;
                result.Code = source.Code;
            }
            if (options.MapObjects)
            {
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override WagonType ReverseMapCore(WagonTypeDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new WagonType();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Name = source.Name;
                result.ShortName = source.ShortName;
                result.Code = source.Code;
            }
            if (options.MapObjects)
            {
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override void MapCore(WagonType source, WagonType destination, MapOptions options = null)
        {
            if (source == null || destination == null)
                return;

            options = options ?? new MapOptions();

            destination.Id = source.Id;
            if (options.MapProperties)
            {
                destination.Name = source.Name;
                destination.ShortName = source.ShortName;
                destination.Code = source.Code;
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
