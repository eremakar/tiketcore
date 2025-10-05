using Data.Mapping;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Models.Dtos;

namespace Ticketing.Mappings
{
    /// <summary>
    /// Особенности модели вагона
    /// </summary>
    public partial class WagonModelFeatureMap : MapBase2<WagonModelFeature, WagonModelFeatureDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public WagonModelFeatureMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override WagonModelFeatureDto MapCore(WagonModelFeature source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new WagonModelFeatureDto();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.WagonId = source.WagonId;
                result.FeatureId = source.FeatureId;
            }
            if (options.MapObjects)
            {
                result.Wagon = mapContext.WagonModelMap.Map(source.Wagon, options);
                result.Feature = mapContext.WagonFeatureMap.Map(source.Feature, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override WagonModelFeature ReverseMapCore(WagonModelFeatureDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new WagonModelFeature();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.WagonId = source.WagonId;
                result.FeatureId = source.FeatureId;
            }
            if (options.MapObjects)
            {
                if (source.WagonId == null)
                    result.Wagon = mapContext.WagonModelMap.ReverseMap(source.Wagon, options);
                if (source.FeatureId == null)
                    result.Feature = mapContext.WagonFeatureMap.ReverseMap(source.Feature, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override void MapCore(WagonModelFeature source, WagonModelFeature destination, MapOptions options = null)
        {
            if (source == null || destination == null)
                return;

            options = options ?? new MapOptions();

            destination.Id = source.Id;
            if (options.MapProperties)
            {
                destination.WagonId = source.WagonId;
                destination.FeatureId = source.FeatureId;
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
