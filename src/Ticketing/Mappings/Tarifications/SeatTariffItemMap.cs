using Data.Mapping;
using Ticketing.Data.TicketDb.Entities.Tarifications;
using Ticketing.Models.Dtos.Tarifications;
using Newtonsoft.Json;
using Data.Repository.Helpers;

namespace Ticketing.Mappings.Tarifications
{
    /// <summary>
    /// Элемент тарифа места
    /// </summary>
    public partial class SeatTariffItemMap : MapBase2<SeatTariffItem, SeatTariffItemDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public SeatTariffItemMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override SeatTariffItemDto MapCore(SeatTariffItem source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new SeatTariffItemDto();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.CalculationParameters = source.CalculationParameters;
                result.Distance = source.Distance;
                result.Price = source.Price;
                result.WagonClassId = source.WagonClassId;
                result.SeasonId = source.SeasonId;
                result.WagonId = source.WagonId;
                result.SeatTypeId = source.SeatTypeId;
                result.FromId = source.FromId;
                result.ToId = source.ToId;
                result.SeatTariffId = source.SeatTariffId;
            }
            if (options.MapObjects)
            {
                result.WagonClass = mapContext.WagonClassMap.Map(source.WagonClass, options);
                result.Season = mapContext.SeasonMap.Map(source.Season, options);
                result.Wagon = mapContext.WagonModelMap.Map(source.Wagon, options);
                result.SeatType = mapContext.SeatTypeMap.Map(source.SeatType, options);
                result.From = mapContext.StationMap.Map(source.From, options);
                result.To = mapContext.StationMap.Map(source.To, options);
                result.SeatTariff = mapContext.SeatTariffMap.Map(source.SeatTariff, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override SeatTariffItem ReverseMapCore(SeatTariffItemDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new SeatTariffItem();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                if (source.CalculationParameters != null)
                    result.CalculationParameters = JsonConvert.SerializeObject(source.CalculationParameters);
                result.Distance = source.Distance;
                result.Price = source.Price;
                result.WagonClassId = source.WagonClassId;
                result.SeasonId = source.SeasonId;
                result.WagonId = source.WagonId;
                result.SeatTypeId = source.SeatTypeId;
                result.FromId = source.FromId;
                result.ToId = source.ToId;
                result.SeatTariffId = source.SeatTariffId;
            }
            if (options.MapObjects)
            {
                if (source.WagonClassId == null)
                    result.WagonClass = mapContext.WagonClassMap.ReverseMap(source.WagonClass, options);
                if (source.SeasonId == null)
                    result.Season = mapContext.SeasonMap.ReverseMap(source.Season, options);
                if (source.WagonId == null)
                    result.Wagon = mapContext.WagonModelMap.ReverseMap(source.Wagon, options);
                if (source.SeatTypeId == null)
                    result.SeatType = mapContext.SeatTypeMap.ReverseMap(source.SeatType, options);
                if (source.FromId == null)
                    result.From = mapContext.StationMap.ReverseMap(source.From, options);
                if (source.ToId == null)
                    result.To = mapContext.StationMap.ReverseMap(source.To, options);
                if (source.SeatTariffId == null)
                    result.SeatTariff = mapContext.SeatTariffMap.ReverseMap(source.SeatTariff, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override void MapCore(SeatTariffItem source, SeatTariffItem destination, MapOptions options = null)
        {
            if (source == null || destination == null)
                return;

            options = options ?? new MapOptions();

            destination.Id = source.Id;
            if (options.MapProperties)
            {
                destination.CalculationParameters = JsonHelper.NormalizeSafe(source.CalculationParameters);
                destination.Distance = source.Distance;
                destination.Price = source.Price;
                destination.WagonClassId = source.WagonClassId;
                destination.SeasonId = source.SeasonId;
                destination.WagonId = source.WagonId;
                destination.SeatTypeId = source.SeatTypeId;
                destination.FromId = source.FromId;
                destination.ToId = source.ToId;
                destination.SeatTariffId = source.SeatTariffId;
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
