using Data.Mapping;
using Data.Repository.Helpers;
using Ticketing.Tarifications.Data.TicketDb.Entities.Tarifications;
using Ticketing.Tarifications.Models.Dtos.Tarifications;

namespace Ticketing.Tarifications.Mappings.Tarifications
{
    /// <summary>
    /// История тарифа места в вагоне
    /// </summary>
    public partial class SeatTariffHistoryMap : MapBase2<SeatTariffHistory, SeatTariffHistoryDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public SeatTariffHistoryMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override SeatTariffHistoryDto MapCore(SeatTariffHistory source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new SeatTariffHistoryDto();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Name = source.Name;
                result.Price = source.Price;
                result.DateTime = source.DateTime;
                result.BaseFareId = source.BaseFareId;
                result.TrainId = source.TrainId;
                result.TrainCategoryId = source.TrainCategoryId;
                result.WagonClassId = source.WagonClassId;
                result.SeasonId = source.SeasonId;
                result.SeatTypeId = source.SeatTypeId;
                result.ConnectionId = source.ConnectionId;
            }
            if (options.MapObjects)
            {
                result.BaseFare = mapContext.BaseFareMap.Map(source.BaseFare, options);
                result.Train = mapContext.TrainMap.Map(source.Train, options);
                result.TrainCategory = mapContext.TrainCategoryMap.Map(source.TrainCategory, options);
                result.WagonClass = mapContext.WagonClassMap.Map(source.WagonClass, options);
                result.Season = mapContext.SeasonMap.Map(source.Season, options);
                result.SeatType = mapContext.SeatTypeMap.Map(source.SeatType, options);
                result.Connection = mapContext.ConnectionMap.Map(source.Connection, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override SeatTariffHistory ReverseMapCore(SeatTariffHistoryDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new SeatTariffHistory();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Name = source.Name;
                result.Price = source.Price;
                result.DateTime = source.DateTime.ToUtc();
                result.BaseFareId = source.BaseFareId;
                result.TrainId = source.TrainId;
                result.TrainCategoryId = source.TrainCategoryId;
                result.WagonClassId = source.WagonClassId;
                result.SeasonId = source.SeasonId;
                result.SeatTypeId = source.SeatTypeId;
                result.ConnectionId = source.ConnectionId;
            }
            if (options.MapObjects)
            {
                if (source.BaseFareId == null)
                    result.BaseFare = mapContext.BaseFareMap.ReverseMap(source.BaseFare, options);
                if (source.TrainId == null)
                    result.Train = mapContext.TrainMap.ReverseMap(source.Train, options);
                if (source.TrainCategoryId == null)
                    result.TrainCategory = mapContext.TrainCategoryMap.ReverseMap(source.TrainCategory, options);
                if (source.WagonClassId == null)
                    result.WagonClass = mapContext.WagonClassMap.ReverseMap(source.WagonClass, options);
                if (source.SeasonId == null)
                    result.Season = mapContext.SeasonMap.ReverseMap(source.Season, options);
                if (source.SeatTypeId == null)
                    result.SeatType = mapContext.SeatTypeMap.ReverseMap(source.SeatType, options);
                if (source.ConnectionId == null)
                    result.Connection = mapContext.ConnectionMap.ReverseMap(source.Connection, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override void MapCore(SeatTariffHistory source, SeatTariffHistory destination, MapOptions options = null)
        {
            if (source == null || destination == null)
                return;

            options = options ?? new MapOptions();

            destination.Id = source.Id;
            if (options.MapProperties)
            {
                destination.Name = source.Name;
                destination.Price = source.Price;
                destination.DateTime = source.DateTime;
                destination.BaseFareId = source.BaseFareId;
                destination.TrainId = source.TrainId;
                destination.TrainCategoryId = source.TrainCategoryId;
                destination.WagonClassId = source.WagonClassId;
                destination.SeasonId = source.SeasonId;
                destination.SeatTypeId = source.SeatTypeId;
                destination.ConnectionId = source.ConnectionId;
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
