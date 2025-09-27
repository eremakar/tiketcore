using Data.Mapping;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Models.Dtos;

namespace Ticketing.Mappings
{
    /// <summary>
    /// Сервис/услуга
    /// </summary>
    public partial class ServiceMap : MapBase2<Service, ServiceDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public ServiceMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override ServiceDto MapCore(Service source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new ServiceDto();
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

        public override Service ReverseMapCore(ServiceDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new Service();
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

        public override void MapCore(Service source, Service destination, MapOptions options = null)
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
