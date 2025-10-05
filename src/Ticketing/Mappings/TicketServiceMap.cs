using Data.Mapping;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Models.Dtos;

namespace Ticketing.Mappings
{
    /// <summary>
    /// Услуги билета
    /// </summary>
    public partial class TicketServiceMap : MapBase2<TicketService, TicketServiceDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public TicketServiceMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override TicketServiceDto MapCore(TicketService source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new TicketServiceDto();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.State = source.State;
                result.TicketId = source.TicketId;
                result.ServiceId = source.ServiceId;
            }
            if (options.MapObjects)
            {
                result.Ticket = mapContext.TicketMap.Map(source.Ticket, options);
                result.Service = mapContext.ServiceMap.Map(source.Service, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override TicketService ReverseMapCore(TicketServiceDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new TicketService();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.State = source.State;
                result.TicketId = source.TicketId;
                result.ServiceId = source.ServiceId;
            }
            if (options.MapObjects)
            {
                if (source.TicketId == null)
                    result.Ticket = mapContext.TicketMap.ReverseMap(source.Ticket, options);
                if (source.ServiceId == null)
                    result.Service = mapContext.ServiceMap.ReverseMap(source.Service, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override void MapCore(TicketService source, TicketService destination, MapOptions options = null)
        {
            if (source == null || destination == null)
                return;

            options = options ?? new MapOptions();

            destination.Id = source.Id;
            if (options.MapProperties)
            {
                destination.State = source.State;
                destination.TicketId = source.TicketId;
                destination.ServiceId = source.ServiceId;
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
