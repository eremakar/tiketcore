using Data.Mapping;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Models.Dtos;

namespace Ticketing.Mappings
{
    /// <summary>
    /// Особенности вагона
    /// </summary>
    public partial class WagonFeatureMap : MapBase2<WagonFeature, WagonFeatureDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public WagonFeatureMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override WagonFeatureDto MapCore(WagonFeature source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new WagonFeatureDto();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Name = source.Name;
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

        public override WagonFeature ReverseMapCore(WagonFeatureDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new WagonFeature();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Name = source.Name;
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

        public override void MapCore(WagonFeature source, WagonFeature destination, MapOptions options = null)
        {
            if (source == null || destination == null)
                return;

            options = options ?? new MapOptions();

            destination.Id = source.Id;
            if (options.MapProperties)
            {
                destination.Name = source.Name;
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
