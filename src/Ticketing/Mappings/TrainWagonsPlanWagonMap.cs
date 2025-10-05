using Data.Mapping;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Models.Dtos;

namespace Ticketing.Mappings
{
    /// <summary>
    /// Вагон в плане состава
    /// </summary>
    public partial class TrainWagonsPlanWagonMap : MapBase2<TrainWagonsPlanWagon, TrainWagonsPlanWagonDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public TrainWagonsPlanWagonMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override TrainWagonsPlanWagonDto MapCore(TrainWagonsPlanWagon source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new TrainWagonsPlanWagonDto();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Number = source.Number;
                result.PlanId = source.PlanId;
                result.WagonId = source.WagonId;
            }
            if (options.MapObjects)
            {
                result.Plan = mapContext.TrainWagonsPlanMap.Map(source.Plan, options);
                result.Wagon = mapContext.WagonModelMap.Map(source.Wagon, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override TrainWagonsPlanWagon ReverseMapCore(TrainWagonsPlanWagonDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new TrainWagonsPlanWagon();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Number = source.Number;
                result.PlanId = source.PlanId;
                result.WagonId = source.WagonId;
            }
            if (options.MapObjects)
            {
                if (source.PlanId == null)
                    result.Plan = mapContext.TrainWagonsPlanMap.ReverseMap(source.Plan, options);
                if (source.WagonId == null)
                    result.Wagon = mapContext.WagonModelMap.ReverseMap(source.Wagon, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override void MapCore(TrainWagonsPlanWagon source, TrainWagonsPlanWagon destination, MapOptions options = null)
        {
            if (source == null || destination == null)
                return;

            options = options ?? new MapOptions();

            destination.Id = source.Id;
            if (options.MapProperties)
            {
                destination.Number = source.Number;
                destination.PlanId = source.PlanId;
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
