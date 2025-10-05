using Data.Mapping;
using Ticketing.Data.TicketDb.Entities.Tarifications;
using Ticketing.Models.Dtos.Tarifications;

namespace Ticketing.Mappings.Tarifications
{
    /// <summary>
    /// Элемент тарифа категории поезда
    /// </summary>
    public partial class TariffTrainCategoryItemMap : MapBase2<TariffTrainCategoryItem, TariffTrainCategoryItemDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public TariffTrainCategoryItemMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override TariffTrainCategoryItemDto MapCore(TariffTrainCategoryItem source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new TariffTrainCategoryItemDto();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.IndexCoefficient = source.IndexCoefficient;
                result.TrainCategoryId = source.TrainCategoryId;
                result.TariffId = source.TariffId;
            }
            if (options.MapObjects)
            {
                result.TrainCategory = mapContext.TrainCategoryMap.Map(source.TrainCategory, options);
                result.Tariff = mapContext.TariffMap.Map(source.Tariff, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override TariffTrainCategoryItem ReverseMapCore(TariffTrainCategoryItemDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new TariffTrainCategoryItem();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.IndexCoefficient = source.IndexCoefficient;
                result.TrainCategoryId = source.TrainCategoryId;
                result.TariffId = source.TariffId;
            }
            if (options.MapObjects)
            {
                if (source.TrainCategoryId == null)
                    result.TrainCategory = mapContext.TrainCategoryMap.ReverseMap(source.TrainCategory, options);
                if (source.TariffId == null)
                    result.Tariff = mapContext.TariffMap.ReverseMap(source.Tariff, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override void MapCore(TariffTrainCategoryItem source, TariffTrainCategoryItem destination, MapOptions options = null)
        {
            if (source == null || destination == null)
                return;

            options = options ?? new MapOptions();

            destination.Id = source.Id;
            if (options.MapProperties)
            {
                destination.IndexCoefficient = source.IndexCoefficient;
                destination.TrainCategoryId = source.TrainCategoryId;
                destination.TariffId = source.TariffId;
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
