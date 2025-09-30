using Data.Mapping;
using Ticketing.Tarifications.Data.TicketDb.Entities;
using Ticketing.Tarifications.Models.Dtos;
using Newtonsoft.Json;
using Data.Repository.Helpers;

namespace Ticketing.Tarifications.Mappings
{
    /// <summary>
    /// Вагон (тип)
    /// </summary>
    public partial class WagonMap : MapBase2<Wagon, WagonDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public WagonMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override WagonDto MapCore(Wagon source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new WagonDto();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.SeatCount = source.SeatCount;
                result.PictureS3 = source.PictureS3;
                result.Class = source.Class;
                result.TypeId = source.TypeId;
            }
            if (options.MapObjects)
            {
                result.Type = mapContext.WagonTypeMap.Map(source.Type, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override Wagon ReverseMapCore(WagonDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new Wagon();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.SeatCount = source.SeatCount;
                if (source.PictureS3 != null)
                    result.PictureS3 = JsonConvert.SerializeObject(source.PictureS3);
                result.Class = source.Class;
                result.TypeId = source.TypeId;
            }
            if (options.MapObjects)
            {
                result.Type = mapContext.WagonTypeMap.ReverseMap(source.Type, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override void MapCore(Wagon source, Wagon destination, MapOptions options = null)
        {
            if (source == null || destination == null)
                return;

            options = options ?? new MapOptions();

            destination.Id = source.Id;
            if (options.MapProperties)
            {
                destination.SeatCount = source.SeatCount;
                destination.PictureS3 = JsonHelper.NormalizeSafe(source.PictureS3);
                destination.Class = source.Class;
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
