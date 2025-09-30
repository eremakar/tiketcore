using Data.Mapping;
using Ticketing.Tarifications.Data.TicketDb.Entities;
using Ticketing.Tarifications.Models.Dtos;

namespace Ticketing.Tarifications.Mappings
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
            }
            if (options.MapObjects)
            {
                result.TrainSchedule = mapContext.TrainScheduleMap.Map(source.TrainSchedule, options);
                result.Wagon = mapContext.WagonMap.Map(source.Wagon, options);
            }
            if (options.MapCollections)
            {
                result.Seats = mapContext.SeatMap.Map(source.Seats, options);
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
            }
            if (options.MapObjects)
            {
                result.TrainSchedule = mapContext.TrainScheduleMap.ReverseMap(source.TrainSchedule, options);
                result.Wagon = mapContext.WagonMap.ReverseMap(source.Wagon, options);
            }
            if (options.MapCollections)
            {
                result.Seats = mapContext.SeatMap.ReverseMap(source.Seats, options);
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
