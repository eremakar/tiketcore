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
                result.IndexCoefficient = source.IndexCoefficient;
                result.VAT = source.VAT;
                result.BaseFareId = source.BaseFareId;
                result.TrainCategoryId = source.TrainCategoryId;
                result.WagonId = source.WagonId;
            }
            if (options.MapObjects)
            {
                result.BaseFare = mapContext.BaseFareMap.Map(source.BaseFare, options);
                result.TrainCategory = mapContext.TrainCategoryMap.Map(source.TrainCategory, options);
                result.Wagon = mapContext.WagonMap.Map(source.Wagon, options);
            }
            if (options.MapCollections)
            {
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
                result.IndexCoefficient = source.IndexCoefficient;
                result.VAT = source.VAT;
                result.BaseFareId = source.BaseFareId;
                result.TrainCategoryId = source.TrainCategoryId;
                result.WagonId = source.WagonId;
            }
            if (options.MapObjects)
            {
                result.BaseFare = mapContext.BaseFareMap.ReverseMap(source.BaseFare, options);
                result.TrainCategory = mapContext.TrainCategoryMap.ReverseMap(source.TrainCategory, options);
                result.Wagon = mapContext.WagonMap.ReverseMap(source.Wagon, options);
            }
            if (options.MapCollections)
            {
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
                destination.IndexCoefficient = source.IndexCoefficient;
                destination.VAT = source.VAT;
                destination.BaseFareId = source.BaseFareId;
                destination.TrainCategoryId = source.TrainCategoryId;
                destination.WagonId = source.WagonId;
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
