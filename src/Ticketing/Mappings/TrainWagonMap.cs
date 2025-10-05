using Data.Mapping;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Models.Dtos;

namespace Ticketing.Mappings
{
    /// <summary>
    /// Вагон состава поезда
    /// </summary>
    public partial class TrainWagonMap : MapBase2<TrainWagon, TrainWagonDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public TrainWagonMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override TrainWagonDto MapCore(TrainWagon source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new TrainWagonDto();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Number = source.Number;
                result.TrainScheduleId = source.TrainScheduleId;
                result.WagonId = source.WagonId;
                result.CarrierId = source.CarrierId;
            }
            if (options.MapObjects)
            {
                result.TrainSchedule = mapContext.TrainScheduleMap.Map(source.TrainSchedule, options);
                result.Wagon = mapContext.WagonModelMap.Map(source.Wagon, options);
                result.Carrier = mapContext.CarrierMap.Map(source.Carrier, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override TrainWagon ReverseMapCore(TrainWagonDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new TrainWagon();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Number = source.Number;
                result.TrainScheduleId = source.TrainScheduleId;
                result.WagonId = source.WagonId;
                result.CarrierId = source.CarrierId;
            }
            if (options.MapObjects)
            {
                if (source.TrainScheduleId == null)
                    result.TrainSchedule = mapContext.TrainScheduleMap.ReverseMap(source.TrainSchedule, options);
                if (source.WagonId == null)
                    result.Wagon = mapContext.WagonModelMap.ReverseMap(source.Wagon, options);
                if (source.CarrierId == null)
                    result.Carrier = mapContext.CarrierMap.ReverseMap(source.Carrier, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override void MapCore(TrainWagon source, TrainWagon destination, MapOptions options = null)
        {
            if (source == null || destination == null)
                return;

            options = options ?? new MapOptions();

            destination.Id = source.Id;
            if (options.MapProperties)
            {
                destination.Number = source.Number;
                destination.TrainScheduleId = source.TrainScheduleId;
                destination.WagonId = source.WagonId;
                destination.CarrierId = source.CarrierId;
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
