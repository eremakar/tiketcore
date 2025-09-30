using Data.Mapping;
using Ticketing.Tarifications.Data.TicketDb.Entities;
using Ticketing.Tarifications.Models.Dtos;

namespace Ticketing.Tarifications.Mappings
{
    /// <summary>
    /// Вокзал
    /// </summary>
    public partial class DepotMap : MapBase2<Depot, DepotDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public DepotMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override DepotDto MapCore(Depot source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new DepotDto();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Name = source.Name;
                result.StationId = source.StationId;
            }
            if (options.MapObjects)
            {
                result.Station = mapContext.StationMap.Map(source.Station, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override Depot ReverseMapCore(DepotDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new Depot();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Name = source.Name;
                result.StationId = source.StationId;
            }
            if (options.MapObjects)
            {
                result.Station = mapContext.StationMap.ReverseMap(source.Station, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override void MapCore(Depot source, Depot destination, MapOptions options = null)
        {
            if (source == null || destination == null)
                return;

            options = options ?? new MapOptions();

            destination.Id = source.Id;
            if (options.MapProperties)
            {
                destination.Name = source.Name;
                destination.StationId = source.StationId;
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
