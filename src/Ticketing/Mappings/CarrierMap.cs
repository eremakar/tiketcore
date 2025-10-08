using Data.Mapping;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Models.Dtos;
using Newtonsoft.Json;
using Data.Repository.Helpers;

namespace Ticketing.Mappings
{
    /// <summary>
    /// Перевозчик
    /// </summary>
    public partial class CarrierMap : MapBase2<Carrier, CarrierDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public CarrierMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override CarrierDto MapCore(Carrier source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new CarrierDto();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Name = source.Name;
                result.BIN = source.BIN;
                result.Description = source.Description;
                result.Filial = source.Filial;
                result.Logo = source.Logo;
            }
            if (options.MapObjects)
            {
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override Carrier ReverseMapCore(CarrierDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new Carrier();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Name = source.Name;
                result.BIN = source.BIN;
                result.Description = source.Description;
                result.Filial = source.Filial;
                if (source.Logo != null)
                    result.Logo = JsonConvert.SerializeObject(source.Logo);
            }
            if (options.MapObjects)
            {
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override void MapCore(Carrier source, Carrier destination, MapOptions options = null)
        {
            if (source == null || destination == null)
                return;

            options = options ?? new MapOptions();

            destination.Id = source.Id;
            if (options.MapProperties)
            {
                destination.Name = source.Name;
                destination.BIN = source.BIN;
                destination.Description = source.Description;
                destination.Filial = source.Filial;
                destination.Logo = JsonHelper.NormalizeSafe(source.Logo);
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
