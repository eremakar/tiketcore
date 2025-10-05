using Data.Mapping;
using Ticketing.Data.TicketDb.Entities.Tarifications;
using Ticketing.Models.Dtos.Tarifications;

namespace Ticketing.Mappings.Tarifications
{
    /// <summary>
    /// Тариф
    /// </summary>
    public partial class TariffMap : MapBase2<Tariff, TariffDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public TariffMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override TariffDto MapCore(Tariff source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new TariffDto();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Name = source.Name;
                result.VAT = source.VAT;
                result.BaseFareId = source.BaseFareId;
            }
            if (options.MapObjects)
            {
                result.BaseFare = mapContext.BaseFareMap.Map(source.BaseFare, options);
            }
            if (options.MapCollections)
            {
                result.TrainCategories = mapContext.TariffTrainCategoryItemMap.Map(source.TrainCategories, options);
                result.Wagons = mapContext.TariffWagonItemMap.Map(source.Wagons, options);
                result.WagonTypes = mapContext.TariffWagonTypeItemMap.Map(source.WagonTypes, options);
            }

            return result;
        }

        public override Tariff ReverseMapCore(TariffDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new Tariff();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Name = source.Name;
                result.VAT = source.VAT;
                result.BaseFareId = source.BaseFareId;
            }
            if (options.MapObjects)
            {
                if (source.BaseFareId == null)
                    result.BaseFare = mapContext.BaseFareMap.ReverseMap(source.BaseFare, options);
            }
            if (options.MapCollections)
            {
                result.TrainCategories = mapContext.TariffTrainCategoryItemMap.ReverseMap(source.TrainCategories, options);
                result.Wagons = mapContext.TariffWagonItemMap.ReverseMap(source.Wagons, options);
                result.WagonTypes = mapContext.TariffWagonTypeItemMap.ReverseMap(source.WagonTypes, options);
            }

            return result;
        }

        public override void MapCore(Tariff source, Tariff destination, MapOptions options = null)
        {
            if (source == null || destination == null)
                return;

            options = options ?? new MapOptions();

            destination.Id = source.Id;
            if (options.MapProperties)
            {
                destination.Name = source.Name;
                destination.VAT = source.VAT;
                destination.BaseFareId = source.BaseFareId;
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
