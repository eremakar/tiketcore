using Data.Mapping;
using Ticketing.Tarifications.Data.TicketDb.Entities;
using Ticketing.Tarifications.Models.Dtos;

namespace Ticketing.Tarifications.Mappings
{
    /// <summary>
    /// Соединение 2х станций
    /// </summary>
    public partial class ConnectionMap : MapBase2<Connection, ConnectionDto, MapOptions>
    {
        private readonly DbMapContext mapContext;

        public ConnectionMap(DbMapContext mapContext)
        {
            this.mapContext = mapContext;
        }

        public override ConnectionDto MapCore(Connection source, MapOptions? options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new ConnectionDto();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Name = source.Name;
                result.DistanceKm = source.DistanceKm;
                result.FromId = source.FromId;
                result.ToId = source.ToId;
            }
            if (options.MapObjects)
            {
                result.From = mapContext.StationMap.Map(source.From, options);
                result.To = mapContext.StationMap.Map(source.To, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override Connection ReverseMapCore(ConnectionDto source, MapOptions options = null)
        {
            if (source == null)
                return null;

            options = options ?? new MapOptions();

            var result = new Connection();
            result.Id = source.Id;
            if (options.MapProperties)
            {
                result.Name = source.Name;
                result.DistanceKm = source.DistanceKm;
                result.FromId = source.FromId;
                result.ToId = source.ToId;
            }
            if (options.MapObjects)
            {
                result.From = mapContext.StationMap.ReverseMap(source.From, options);
                result.To = mapContext.StationMap.ReverseMap(source.To, options);
            }
            if (options.MapCollections)
            {
            }

            return result;
        }

        public override void MapCore(Connection source, Connection destination, MapOptions options = null)
        {
            if (source == null || destination == null)
                return;

            options = options ?? new MapOptions();

            destination.Id = source.Id;
            if (options.MapProperties)
            {
                destination.Name = source.Name;
                destination.DistanceKm = source.DistanceKm;
                destination.FromId = source.FromId;
                destination.ToId = source.ToId;
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
