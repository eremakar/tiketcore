using Data.Mapping;
using Ticketing.Data.TicketDb.Entities.Dictionaries;
using Ticketing.Models.Dtos.Dictionaries;

namespace Ticketing.Mappings.Dictionaries
{
    /// <summary>
    /// Тип поезда
    /// </summary>
    public partial class TrainTypeMap : MapBase2<TrainType, TrainTypeDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public TrainTypeMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override TrainTypeDto MapCore(TrainType source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new TrainTypeDto();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Name = source.Name;
                result.Code = source.Code;
            }
            if (options.MapObjects)
            {
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override TrainType ReverseMapCore(TrainTypeDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new TrainType();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Name = source.Name;
                result.Code = source.Code;
            }
            if (options.MapObjects)
            {
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override void MapCore(TrainType source, TrainType destination, MapOptions options = null)
        {
            if (source == null || destination == null)
                return;

            options = options ?? new MapOptions();

            destination.Id = source.Id;
            if (options.MapProperties)
            {
                destination.Name = source.Name;
                destination.Code = source.Code;
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
