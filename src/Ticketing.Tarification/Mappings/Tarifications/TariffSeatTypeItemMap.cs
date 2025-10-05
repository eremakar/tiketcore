using Data.Mapping;
using Ticketing.Tarifications.Data.TicketDb.Entities.Tarifications;
using Ticketing.Tarifications.Models.Dtos.Tarifications;

namespace Ticketing.Tarifications.Mappings.Tarifications
{
    /// <summary>
    /// Элемент тарифа типа места
    /// </summary>
    public partial class TariffSeatTypeItemMap : MapBase2<TariffSeatTypeItem, TariffSeatTypeItemDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public TariffSeatTypeItemMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override TariffSeatTypeItemDto MapCore(TariffSeatTypeItem source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new TariffSeatTypeItemDto();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.IndexCoefficient = source.IndexCoefficient;
                result.SeatTypeId = source.SeatTypeId;
                result.TariffWagonId = source.TariffWagonId;
            }
            if (options.MapObjects)
            {
                result.SeatType = mapContext.SeatTypeMap.Map(source.SeatType, options);
                result.TariffWagon = mapContext.TariffWagonItemMap.Map(source.TariffWagon, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override TariffSeatTypeItem ReverseMapCore(TariffSeatTypeItemDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new TariffSeatTypeItem();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.IndexCoefficient = source.IndexCoefficient;
                result.SeatTypeId = source.SeatTypeId;
                result.TariffWagonId = source.TariffWagonId;
            }
            if (options.MapObjects)
            {
                if (source.SeatTypeId == null)
                    result.SeatType = mapContext.SeatTypeMap.ReverseMap(source.SeatType, options);
                if (source.TariffWagonId == null)
                    result.TariffWagon = mapContext.TariffWagonItemMap.ReverseMap(source.TariffWagon, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override void MapCore(TariffSeatTypeItem source, TariffSeatTypeItem destination, MapOptions options = null)
        {
            if (source == null || destination == null)
                return;

            options = options ?? new MapOptions();

            destination.Id = source.Id;
            if (options.MapProperties)
            {
                destination.IndexCoefficient = source.IndexCoefficient;
                destination.SeatTypeId = source.SeatTypeId;
                destination.TariffWagonId = source.TariffWagonId;
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
