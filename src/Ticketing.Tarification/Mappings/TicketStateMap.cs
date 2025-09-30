using Data.Mapping;
using Ticketing.Tarifications.Data.TicketDb.Entities;
using Ticketing.Tarifications.Models.Dtos;

namespace Ticketing.Tarifications.Mappings
{
    /// <summary>
    /// Статус билета
    /// </summary>
    public partial class TicketStateMap : MapBase2<TicketState, TicketStateDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public TicketStateMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override TicketStateDto MapCore(TicketState source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new TicketStateDto();
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

        public override TicketState ReverseMapCore(TicketStateDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new TicketState();
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

        public override void MapCore(TicketState source, TicketState destination, MapOptions options = null)
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
