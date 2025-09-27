using Data.Mapping;
using Ticketing.Data.TicketDb.Entities.Tarifications;
using Ticketing.Models.Dtos.Tarifications;

namespace Ticketing.Mappings.Tarifications
{
    /// <summary>
    /// Категория поезда
    /// </summary>
    public partial class TrainCategoryMap : MapBase2<TrainCategory, TrainCategoryDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public TrainCategoryMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override TrainCategoryDto MapCore(TrainCategory source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new TrainCategoryDto();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Name = source.Name;
                result.Code = source.Code;
                result.TarifCoefficient = source.TarifCoefficient;
            }
            if (options.MapObjects)
            {
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override TrainCategory ReverseMapCore(TrainCategoryDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new TrainCategory();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Name = source.Name;
                result.Code = source.Code;
                result.TarifCoefficient = source.TarifCoefficient;
            }
            if (options.MapObjects)
            {
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override void MapCore(TrainCategory source, TrainCategory destination, MapOptions options = null)
        {
            if (source == null || destination == null)
                return;

            options = options ?? new MapOptions();

            destination.Id = source.Id;
            if (options.MapProperties)
            {
                destination.Name = source.Name;
                destination.Code = source.Code;
                destination.TarifCoefficient = source.TarifCoefficient;
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
