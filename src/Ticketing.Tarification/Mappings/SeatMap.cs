using Data.Mapping;
using Ticketing.Tarifications.Data.TicketDb.Entities;
using Ticketing.Tarifications.Models.Dtos;

namespace Ticketing.Tarifications.Mappings
{
    /// <summary>
    /// Место в вагоне
    /// </summary>
    public partial class SeatMap : MapBase2<Seat, SeatDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public SeatMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override SeatDto MapCore(Seat source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new SeatDto();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Number = source.Number;
                result.Class = source.Class;
                result.WagonId = source.WagonId;
                result.TypeId = source.TypeId;
            }
            if (options.MapObjects)
            {
                result.Wagon = mapContext.TrainWagonMap.Map(source.Wagon, options);
                result.Type = mapContext.SeatTypeMap.Map(source.Type, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override Seat ReverseMapCore(SeatDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new Seat();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Number = source.Number;
                result.Class = source.Class;
                result.WagonId = source.WagonId;
                result.TypeId = source.TypeId;
            }
            if (options.MapObjects)
            {
                result.Wagon = mapContext.TrainWagonMap.ReverseMap(source.Wagon, options);
                result.Type = mapContext.SeatTypeMap.ReverseMap(source.Type, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override void MapCore(Seat source, Seat destination, MapOptions options = null)
        {
            if (source == null || destination == null)
                return;

            options = options ?? new MapOptions();

            destination.Id = source.Id;
            if (options.MapProperties)
            {
                destination.Number = source.Number;
                destination.Class = source.Class;
                destination.WagonId = source.WagonId;
                destination.TypeId = source.TypeId;
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
