using Data.Mapping;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Models.Dtos;

namespace Ticketing.Mappings
{
    /// <summary>
    /// Сегмент поездки (от-до)
    /// </summary>
    public partial class SegmentMap : MapBase2<Segment, SegmentDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public SegmentMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override SegmentDto MapCore(Segment source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new SegmentDto();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.SeatId = source.SeatId;
                result.FromId = source.FromId;
                result.ToId = source.ToId;
                result.TrainId = source.TrainId;
                result.WagonId = source.WagonId;
                result.TrainScheduleId = source.TrainScheduleId;
                result.TicketId = source.TicketId;
                result.ReservationId = source.ReservationId;
            }
            if (options.MapObjects)
            {
                result.Seat = mapContext.SeatMap.Map(source.Seat, options);
                result.From = mapContext.RouteStationMap.Map(source.From, options);
                result.To = mapContext.RouteStationMap.Map(source.To, options);
                result.Train = mapContext.TrainMap.Map(source.Train, options);
                result.Wagon = mapContext.TrainWagonMap.Map(source.Wagon, options);
                result.TrainSchedule = mapContext.TrainScheduleMap.Map(source.TrainSchedule, options);
                result.Ticket = mapContext.TicketMap.Map(source.Ticket, options);
                result.Reservation = mapContext.SeatReservationMap.Map(source.Reservation, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override Segment ReverseMapCore(SegmentDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new Segment();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.SeatId = source.SeatId;
                result.FromId = source.FromId;
                result.ToId = source.ToId;
                result.TrainId = source.TrainId;
                result.WagonId = source.WagonId;
                result.TrainScheduleId = source.TrainScheduleId;
                result.TicketId = source.TicketId;
                result.ReservationId = source.ReservationId;
            }
            if (options.MapObjects)
            {
                result.Seat = mapContext.SeatMap.ReverseMap(source.Seat, options);
                result.From = mapContext.RouteStationMap.ReverseMap(source.From, options);
                result.To = mapContext.RouteStationMap.ReverseMap(source.To, options);
                result.Train = mapContext.TrainMap.ReverseMap(source.Train, options);
                result.Wagon = mapContext.TrainWagonMap.ReverseMap(source.Wagon, options);
                result.TrainSchedule = mapContext.TrainScheduleMap.ReverseMap(source.TrainSchedule, options);
                result.Ticket = mapContext.TicketMap.ReverseMap(source.Ticket, options);
                result.Reservation = mapContext.SeatReservationMap.ReverseMap(source.Reservation, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override void MapCore(Segment source, Segment destination, MapOptions options = null)
        {
            if (source == null || destination == null)
                return;

            options = options ?? new MapOptions();

            destination.Id = source.Id;
            if (options.MapProperties)
            {
                destination.SeatId = source.SeatId;
                destination.FromId = source.FromId;
                destination.ToId = source.ToId;
                destination.TrainId = source.TrainId;
                destination.WagonId = source.WagonId;
                destination.TrainScheduleId = source.TrainScheduleId;
                destination.TicketId = source.TicketId;
                destination.ReservationId = source.ReservationId;
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
