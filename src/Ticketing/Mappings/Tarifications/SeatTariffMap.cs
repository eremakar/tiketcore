using Data.Mapping;
using Ticketing.Data.TicketDb.Entities.Tarifications;
using Ticketing.Models.Dtos.Tarifications;

namespace Ticketing.Mappings.Tarifications
{
    /// <summary>
    /// Тариф места в вагоне
    /// </summary>
    public partial class SeatTariffMap : MapBase2<SeatTariff, SeatTariffDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public SeatTariffMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override SeatTariffDto MapCore(SeatTariff source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new SeatTariffDto();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Name = source.Name;
                result.TrainId = source.TrainId;
                result.BaseFareId = source.BaseFareId;
                result.TrainCategoryId = source.TrainCategoryId;
            }
            if (options.MapObjects)
            {
                result.Train = mapContext.TrainMap.Map(source.Train, options);
                result.BaseFare = mapContext.BaseFareMap.Map(source.BaseFare, options);
                result.TrainCategory = mapContext.TrainCategoryMap.Map(source.TrainCategory, options);
            }
            if (options.MapCollections)
            {
                result.Items = mapContext.SeatTariffItemMap.Map(source.Items, options);
            }

            return result;
        }

        public override SeatTariff ReverseMapCore(SeatTariffDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new SeatTariff();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Name = source.Name;
                result.TrainId = source.TrainId;
                result.BaseFareId = source.BaseFareId;
                result.TrainCategoryId = source.TrainCategoryId;
            }
            if (options.MapObjects)
            {
                result.Train = mapContext.TrainMap.ReverseMap(source.Train, options);
                result.BaseFare = mapContext.BaseFareMap.ReverseMap(source.BaseFare, options);
                result.TrainCategory = mapContext.TrainCategoryMap.ReverseMap(source.TrainCategory, options);
            }
            if (options.MapCollections)
            {
                result.Items = mapContext.SeatTariffItemMap.ReverseMap(source.Items, options);
            }

            return result;
        }

        public override void MapCore(SeatTariff source, SeatTariff destination, MapOptions options = null)
        {
            if (source == null || destination == null)
                return;

            options = options ?? new MapOptions();

            destination.Id = source.Id;
            if (options.MapProperties)
            {
                destination.Name = source.Name;
                destination.TrainId = source.TrainId;
                destination.BaseFareId = source.BaseFareId;
                destination.TrainCategoryId = source.TrainCategoryId;
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
