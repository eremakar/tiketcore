using Data.Mapping;
using Ticketing.Data.TicketDb.Entities.Tarifications;
using Ticketing.Models.Dtos.Tarifications;

namespace Ticketing.Mappings.Tarifications
{
    /// <summary>
    /// Класс вагона
    /// </summary>
    public partial class WagonClassMap : MapBase2<WagonClass, WagonClassDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public WagonClassMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override WagonClassDto MapCore(WagonClass source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new WagonClassDto();
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

        public override WagonClass ReverseMapCore(WagonClassDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new WagonClass();
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

        public override void MapCore(WagonClass source, WagonClass destination, MapOptions options = null)
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
