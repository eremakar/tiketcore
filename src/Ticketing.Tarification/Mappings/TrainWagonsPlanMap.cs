using Data.Mapping;
using Ticketing.Tarifications.Data.TicketDb.Entities;
using Ticketing.Tarifications.Models.Dtos;

namespace Ticketing.Tarifications.Mappings
{
    /// <summary>
    /// План состава поезда
    /// </summary>
    public partial class TrainWagonsPlanMap : MapBase2<TrainWagonsPlan, TrainWagonsPlanDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public TrainWagonsPlanMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override TrainWagonsPlanDto MapCore(TrainWagonsPlan source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new TrainWagonsPlanDto();
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
                result.Wagons = mapContext.TrainWagonsPlanWagonMap.Map(source.Wagons, options);
            }

            return result;
        }

        public override TrainWagonsPlan ReverseMapCore(TrainWagonsPlanDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new TrainWagonsPlan();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Name = source.Name;
                result.TrainId = source.TrainId;
            }
            if (options.MapObjects)
            {
                result.Train = mapContext.TrainMap.ReverseMap(source.Train, options);
            }
            if (options.MapCollections)
            {
                result.Wagons = mapContext.TrainWagonsPlanWagonMap.ReverseMap(source.Wagons, options);
            }

            return result;
        }

        public override void MapCore(TrainWagonsPlan source, TrainWagonsPlan destination, MapOptions options = null)
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
