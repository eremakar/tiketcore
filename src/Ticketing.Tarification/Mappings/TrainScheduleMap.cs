using Data.Mapping;
using Data.Repository.Helpers;
using Ticketing.Tarifications.Data.TicketDb.Entities;
using Ticketing.Tarifications.Models.Dtos;

namespace Ticketing.Tarifications.Mappings
{
    /// <summary>
    /// Расписание поезда по дням
    /// </summary>
    public partial class TrainScheduleMap : MapBase2<TrainSchedule, TrainScheduleDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public TrainScheduleMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override TrainScheduleDto MapCore(TrainSchedule source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new TrainScheduleDto();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Date = source.Date;
                result.Active = source.Active;
                result.TrainId = source.TrainId;
                result.SeatTariffId = source.SeatTariffId;
            }
            if (options.MapObjects)
            {
                result.Train = mapContext.TrainMap.Map(source.Train, options);
                result.SeatTariff = mapContext.SeatTariffMap.Map(source.SeatTariff, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override TrainSchedule ReverseMapCore(TrainScheduleDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new TrainSchedule();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Date = source.Date.ToUtc();
                result.Active = source.Active;
                result.TrainId = source.TrainId;
                result.SeatTariffId = source.SeatTariffId;
            }
            if (options.MapObjects)
            {
                result.Train = mapContext.TrainMap.ReverseMap(source.Train, options);
                result.SeatTariff = mapContext.SeatTariffMap.ReverseMap(source.SeatTariff, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override void MapCore(TrainSchedule source, TrainSchedule destination, MapOptions options = null)
        {
            if (source == null || destination == null)
                return;

            options = options ?? new MapOptions();

            destination.Id = source.Id;
            if (options.MapProperties)
            {
                destination.Date = source.Date;
                destination.Active = source.Active;
                destination.TrainId = source.TrainId;
                destination.SeatTariffId = source.SeatTariffId;
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
