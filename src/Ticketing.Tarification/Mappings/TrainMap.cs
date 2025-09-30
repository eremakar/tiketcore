using Data.Mapping;
using Ticketing.Tarifications.Data.TicketDb.Entities;
using Ticketing.Tarifications.Models.Dtos;

namespace Ticketing.Tarifications.Mappings
{
    /// <summary>
    /// Поезд
    /// </summary>
    public partial class TrainMap : MapBase2<Train, TrainDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public TrainMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override TrainDto MapCore(Train source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new TrainDto();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Name = source.Name;
                result.Type = source.Type;
                result.ZoneType = source.ZoneType;
                result.FromId = source.FromId;
                result.ToId = source.ToId;
                result.RouteId = source.RouteId;
                result.PlanId = source.PlanId;
            }
            if (options.MapObjects)
            {
                result.From = mapContext.StationMap.Map(source.From, options);
                result.To = mapContext.StationMap.Map(source.To, options);
                result.Route = mapContext.RouteMap.Map(source.Route, options);
                result.Plan = mapContext.TrainWagonsPlanMap.Map(source.Plan, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override Train ReverseMapCore(TrainDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new Train();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Name = source.Name;
                result.Type = source.Type;
                result.ZoneType = source.ZoneType;
                result.FromId = source.FromId;
                result.ToId = source.ToId;
                result.RouteId = source.RouteId;
                result.PlanId = source.PlanId;
            }
            if (options.MapObjects)
            {
                result.From = mapContext.StationMap.ReverseMap(source.From, options);
                result.To = mapContext.StationMap.ReverseMap(source.To, options);
                result.Route = mapContext.RouteMap.ReverseMap(source.Route, options);
                result.Plan = mapContext.TrainWagonsPlanMap.ReverseMap(source.Plan, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override void MapCore(Train source, Train destination, MapOptions options = null)
        {
            if (source == null || destination == null)
                return;

            options = options ?? new MapOptions();

            destination.Id = source.Id;
            if (options.MapProperties)
            {
                destination.Name = source.Name;
                destination.Type = source.Type;
                destination.ZoneType = source.ZoneType;
                destination.FromId = source.FromId;
                destination.ToId = source.ToId;
                destination.RouteId = source.RouteId;
                destination.PlanId = source.PlanId;
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
