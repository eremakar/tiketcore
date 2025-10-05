using Data.Mapping;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Models.Dtos;

namespace Ticketing.Mappings
{
    /// <summary>
    /// Назначение места
    /// </summary>
    public partial class SeatPurposeMap : MapBase2<SeatPurpose, SeatPurposeDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public SeatPurposeMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override SeatPurposeDto MapCore(SeatPurpose source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new SeatPurposeDto();
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

        public override SeatPurpose ReverseMapCore(SeatPurposeDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new SeatPurpose();
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

        public override void MapCore(SeatPurpose source, SeatPurpose destination, MapOptions options = null)
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
