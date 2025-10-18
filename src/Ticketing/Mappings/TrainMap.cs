using Data.Mapping;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Models.Dtos;

namespace Ticketing.Mappings
{
    /// <summary>
    /// Поезд по маршруту
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
                result.ZoneType = source.ZoneType;
                result.Importance = source.Importance;
                result.Amenities = source.Amenities;
                result.TypeId = source.TypeId;
                result.FromId = source.FromId;
                result.ToId = source.ToId;
                result.RouteId = source.RouteId;
                result.PeriodicityId = source.PeriodicityId;
                result.PlanId = source.PlanId;
                result.CategoryId = source.CategoryId;
                result.TariffId = source.TariffId;
            }
            if (options.MapObjects)
            {
                result.Type = mapContext.TrainTypeMap.Map(source.Type, options);
                result.From = mapContext.StationMap.Map(source.From, options);
                result.To = mapContext.StationMap.Map(source.To, options);
                result.Route = mapContext.RouteMap.Map(source.Route, options);
                result.Periodicity = mapContext.PeriodicityMap.Map(source.Periodicity, options);
                result.Plan = mapContext.TrainWagonsPlanMap.Map(source.Plan, options);
                result.Category = mapContext.TrainCategoryMap.Map(source.Category, options);
                result.Tariff = mapContext.TariffMap.Map(source.Tariff, options);
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
                result.ZoneType = source.ZoneType;
                result.Importance = source.Importance;
                result.Amenities = source.Amenities;
                result.TypeId = source.TypeId;
                result.FromId = source.FromId;
                result.ToId = source.ToId;
                result.RouteId = source.RouteId;
                result.PeriodicityId = source.PeriodicityId;
                result.PlanId = source.PlanId;
                result.CategoryId = source.CategoryId;
                result.TariffId = source.TariffId;
            }
            if (options.MapObjects)
            {
                if (source.TypeId == null)
                    result.Type = mapContext.TrainTypeMap.ReverseMap(source.Type, options);
                if (source.FromId == null)
                    result.From = mapContext.StationMap.ReverseMap(source.From, options);
                if (source.ToId == null)
                    result.To = mapContext.StationMap.ReverseMap(source.To, options);
                if (source.RouteId == null)
                    result.Route = mapContext.RouteMap.ReverseMap(source.Route, options);
                if (source.PeriodicityId == null)
                    result.Periodicity = mapContext.PeriodicityMap.ReverseMap(source.Periodicity, options);
                if (source.PlanId == null)
                    result.Plan = mapContext.TrainWagonsPlanMap.ReverseMap(source.Plan, options);
                if (source.CategoryId == null)
                    result.Category = mapContext.TrainCategoryMap.ReverseMap(source.Category, options);
                if (source.TariffId == null)
                    result.Tariff = mapContext.TariffMap.ReverseMap(source.Tariff, options);
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
                destination.ZoneType = source.ZoneType;
                destination.Importance = source.Importance;
                destination.Amenities = source.Amenities;
                destination.TypeId = source.TypeId;
                destination.FromId = source.FromId;
                destination.ToId = source.ToId;
                destination.RouteId = source.RouteId;
                destination.PeriodicityId = source.PeriodicityId;
                destination.PlanId = source.PlanId;
                destination.CategoryId = source.CategoryId;
                destination.TariffId = source.TariffId;
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
