using Data.Mapping;
using Ticketing.Tarifications.Data.TicketDb.Entities.Tarifications;
using Ticketing.Tarifications.Models.Dtos.Tarifications;

namespace Ticketing.Tarifications.Mappings.Tarifications
{
    /// <summary>
    /// Сезонность
    /// </summary>
    public partial class SeasonMap : MapBase2<Season, SeasonDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public SeasonMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override SeasonDto MapCore(Season source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new SeasonDto();
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

        public override Season ReverseMapCore(SeasonDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new Season();
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

        public override void MapCore(Season source, Season destination, MapOptions options = null)
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
