using Data.Mapping;
using Ticketing.Tarifications.Data.TicketDb.Entities.Tarifications;
using Ticketing.Tarifications.Models.Dtos.Tarifications;

namespace Ticketing.Tarifications.Mappings.Tarifications
{
    /// <summary>
    /// Элемент тарифа вагона
    /// </summary>
    public partial class TariffWagonItemMap : MapBase2<TariffWagonItem, TariffWagonItemDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public TariffWagonItemMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override TariffWagonItemDto MapCore(TariffWagonItem source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new TariffWagonItemDto();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.IndexCoefficient = source.IndexCoefficient;
                result.WagonId = source.WagonId;
                result.TariffId = source.TariffId;
            }
            if (options.MapObjects)
            {
                result.Wagon = mapContext.WagonModelMap.Map(source.Wagon, options);
                result.Tariff = mapContext.TariffMap.Map(source.Tariff, options);
            }
            if (options.MapCollections)
            {
                result.SeatTypes = mapContext.TariffSeatTypeItemMap.Map(source.SeatTypes, options);
            }

            return result;
        }

        public override TariffWagonItem ReverseMapCore(TariffWagonItemDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new TariffWagonItem();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.IndexCoefficient = source.IndexCoefficient;
                result.WagonId = source.WagonId;
                result.TariffId = source.TariffId;
            }
            if (options.MapObjects)
            {
                if (source.WagonId == null)
                    result.Wagon = mapContext.WagonModelMap.ReverseMap(source.Wagon, options);
                if (source.TariffId == null)
                    result.Tariff = mapContext.TariffMap.ReverseMap(source.Tariff, options);
            }
            if (options.MapCollections)
            {
                result.SeatTypes = mapContext.TariffSeatTypeItemMap.ReverseMap(source.SeatTypes, options);
            }

            return result;
        }

        public override void MapCore(TariffWagonItem source, TariffWagonItem destination, MapOptions options = null)
        {
            if (source == null || destination == null)
                return;

            options = options ?? new MapOptions();

            destination.Id = source.Id;
            if (options.MapProperties)
            {
                destination.IndexCoefficient = source.IndexCoefficient;
                destination.WagonId = source.WagonId;
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
