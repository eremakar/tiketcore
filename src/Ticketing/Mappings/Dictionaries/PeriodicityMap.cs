using Data.Mapping;
using Ticketing.Data.TicketDb.Entities.Dictionaries;
using Ticketing.Models.Dtos.Dictionaries;

namespace Ticketing.Mappings.Dictionaries
{
    /// <summary>
    /// Периодичность
    /// </summary>
    public partial class PeriodicityMap : MapBase2<Periodicity, PeriodicityDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public PeriodicityMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override PeriodicityDto MapCore(Periodicity source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new PeriodicityDto();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Name = source.Name;
                result.Code = source.Code;
            }
            if (options.MapObjects)
            {
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override Periodicity ReverseMapCore(PeriodicityDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new Periodicity();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Name = source.Name;
                result.Code = source.Code;
            }
            if (options.MapObjects)
            {
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override void MapCore(Periodicity source, Periodicity destination, MapOptions options = null)
        {
            if (source == null || destination == null)
                return;

            options = options ?? new MapOptions();

            destination.Id = source.Id;
            if (options.MapProperties)
            {
                destination.Name = source.Name;
                destination.Code = source.Code;
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
