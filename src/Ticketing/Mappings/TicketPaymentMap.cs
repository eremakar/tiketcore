using Data.Mapping;
using Data.Repository.Helpers;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Models.Dtos;

namespace Ticketing.Mappings
{
    /// <summary>
    /// Оплата билета
    /// </summary>
    public partial class TicketPaymentMap : MapBase2<TicketPayment, TicketPaymentDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public TicketPaymentMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override TicketPaymentDto MapCore(TicketPayment source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new TicketPaymentDto();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Time = source.Time;
                result.Price = source.Price;
                result.State = source.State;
                result.TicketId = source.TicketId;
                result.UserId = source.UserId;
            }
            if (options.MapObjects)
            {
                result.Ticket = mapContext.TicketMap.Map(source.Ticket, options);
                result.User = mapContext.UserMap.Map(source.User, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override TicketPayment ReverseMapCore(TicketPaymentDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new TicketPayment();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Time = source.Time.ToUtc();
                result.Price = source.Price;
                result.State = source.State;
                result.TicketId = source.TicketId;
                result.UserId = source.UserId;
            }
            if (options.MapObjects)
            {
                result.Ticket = mapContext.TicketMap.ReverseMap(source.Ticket, options);
                result.User = mapContext.UserMap.ReverseMap(source.User, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override void MapCore(TicketPayment source, TicketPayment destination, MapOptions options = null)
        {
            if (source == null || destination == null)
                return;

            options = options ?? new MapOptions();

            destination.Id = source.Id;
            if (options.MapProperties)
            {
                destination.Time = source.Time;
                destination.Price = source.Price;
                destination.State = source.State;
                destination.TicketId = source.TicketId;
                destination.UserId = source.UserId;
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
