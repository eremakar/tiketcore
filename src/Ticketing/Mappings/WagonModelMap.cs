using Data.Mapping;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Models.Dtos;
using Newtonsoft.Json;
using Data.Repository.Helpers;

namespace Ticketing.Mappings
{
    /// <summary>
    /// Вагон (тип)
    /// </summary>
    public partial class WagonModelMap : MapBase2<WagonModel, WagonModelDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public WagonModelMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override WagonModelDto MapCore(WagonModel source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new WagonModelDto();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Name = source.Name;
                result.SeatCount = source.SeatCount;
                result.PictureS3 = source.PictureS3;
                result.HasLiftingMechanism = source.HasLiftingMechanism;
                result.ManufacturerName = source.ManufacturerName;
                result.ClassId = source.ClassId;
                result.TypeId = source.TypeId;
            }
            if (options.MapObjects)
            {
                if (source.Class != null)
                    result.Class = mapContext.WagonClassMap.Map(source.Class, options);
                if (source.Type != null)
                    result.Type = mapContext.WagonTypeMap.Map(source.Type, options);
            }
            if (options.MapCollections)
            {
                result.Features = mapContext.WagonModelFeatureMap.Map(source.Features, options);
                result.Seats = mapContext.SeatMap.Map(source.Seats, options);
            }

            return result;
        }

        public override WagonModel ReverseMapCore(WagonModelDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new WagonModel();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Name = source.Name;
                result.SeatCount = source.SeatCount;
                if (source.PictureS3 != null)
                    result.PictureS3 = JsonConvert.SerializeObject(source.PictureS3);
                result.HasLiftingMechanism = source.HasLiftingMechanism;
                result.ManufacturerName = source.ManufacturerName;
                result.ClassId = source.ClassId;
                result.TypeId = source.TypeId;
            }
            if (options.MapObjects)
            {
                if (source.ClassId == null)
                    result.Class = mapContext.WagonClassMap.ReverseMap(source.Class, options);
                if (source.TypeId == null)
                    result.Type = mapContext.WagonTypeMap.ReverseMap(source.Type, options);
            }
            if (options.MapCollections)
            {
                result.Features = mapContext.WagonModelFeatureMap.ReverseMap(source.Features, options);
                result.Seats = mapContext.SeatMap.ReverseMap(source.Seats, options);
            }

            return result;
        }

        public override void MapCore(WagonModel source, WagonModel destination, MapOptions options = null)
        {
            if (source == null || destination == null)
                return;

            options = options ?? new MapOptions();

            destination.Id = source.Id;
            if (options.MapProperties)
            {
                destination.Name = source.Name;
                destination.SeatCount = source.SeatCount;
                destination.PictureS3 = JsonHelper.NormalizeSafe(source.PictureS3);
                destination.HasLiftingMechanism = source.HasLiftingMechanism;
                destination.ManufacturerName = source.ManufacturerName;
                destination.ClassId = source.ClassId;
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
