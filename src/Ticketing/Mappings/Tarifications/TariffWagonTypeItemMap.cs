using Data.Mapping;
using Ticketing.Data.TicketDb.Entities.Tarifications;
using Ticketing.Models.Dtos.Tarifications;

namespace Ticketing.Mappings.Tarifications
{
    /// <summary>
    /// Элемент тарифа типа вагона
    /// </summary>
    public partial class TariffWagonTypeItemMap : MapBase2<TariffWagonTypeItem, TariffWagonTypeItemDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public TariffWagonTypeItemMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override TariffWagonTypeItemDto MapCore(TariffWagonTypeItem source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new TariffWagonTypeItemDto();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.IndexCoefficient = source.IndexCoefficient;
                result.WagonTypeId = source.WagonTypeId;
                result.TariffId = source.TariffId;
            }
            if (options.MapObjects)
            {
                result.WagonType = mapContext.WagonTypeMap.Map(source.WagonType, options);
                result.Tariff = mapContext.TariffMap.Map(source.Tariff, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override TariffWagonTypeItem ReverseMapCore(TariffWagonTypeItemDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new TariffWagonTypeItem();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.IndexCoefficient = source.IndexCoefficient;
                result.WagonTypeId = source.WagonTypeId;
                result.TariffId = source.TariffId;
            }
            if (options.MapObjects)
            {
                if (source.WagonTypeId == null)
                    result.WagonType = mapContext.WagonTypeMap.ReverseMap(source.WagonType, options);
                if (source.TariffId == null)
                    result.Tariff = mapContext.TariffMap.ReverseMap(source.Tariff, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override void MapCore(TariffWagonTypeItem source, TariffWagonTypeItem destination, MapOptions options = null)
        {
            if (source == null || destination == null)
                return;

            options = options ?? new MapOptions();

            destination.Id = source.Id;
            if (options.MapProperties)
            {
                destination.IndexCoefficient = source.IndexCoefficient;
                destination.WagonTypeId = source.WagonTypeId;
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
