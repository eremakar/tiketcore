using Data.Mapping;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Models.Dtos;

namespace Ticketing.Mappings
{
    /// <summary>
    /// Маршрут
    /// </summary>
    public partial class RouteMap : MapBase2<Data.TicketDb.Entities.Route, RouteDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public RouteMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override RouteDto MapCore(Data.TicketDb.Entities.Route source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new RouteDto();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Name = source.Name;
                result.TrainId = source.TrainId;
            }
            if (options.MapObjects)
            {
                result.Train = mapContext.TrainMap.Map(source.Train, options);
            }
            if (options.MapCollections)
            {
                result.Stations = mapContext.RouteStationMap.Map(source.Stations, options);
            }

            return result;
        }

        public override Data.TicketDb.Entities.Route ReverseMapCore(RouteDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new Data.TicketDb.Entities.Route();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Name = source.Name;
                result.TrainId = source.TrainId;
            }
            if (options.MapObjects)
            {
                if (source.TrainId == null)
                    result.Train = mapContext.TrainMap.ReverseMap(source.Train, options);
            }
            if (options.MapCollections)
            {
                result.Stations = mapContext.RouteStationMap.ReverseMap(source.Stations, options);
            }

            return result;
        }

        public override void MapCore(Data.TicketDb.Entities.Route source, Data.TicketDb.Entities.Route destination, MapOptions options = null)
        {
            if (source == null || destination == null)
                return;

            options = options ?? new MapOptions();

            destination.Id = source.Id;
            if (options.MapProperties)
            {
                destination.Name = source.Name;
                destination.TrainId = source.TrainId;
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
