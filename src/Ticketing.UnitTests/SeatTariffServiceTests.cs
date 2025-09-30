using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Ticketing.Tarifications.Data.TicketDb.DatabaseContext;
using Ticketing.Tarifications.Data.TicketDb.Entities;
using Ticketing.Tarifications.Data.TicketDb.Entities.Tarifications;
using Ticketing.Tarifications.Services;
using Xunit;

namespace Ticketing.UnitTests
{
    public class SeatTariffServiceTests
    {
        private static TicketDbContext CreateInMemoryDb(string dbName)
        {
            var options = new DbContextOptionsBuilder<TicketDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            var ctx = new TicketDbContext(options);
            return ctx;
        }

        [Fact]
        public async Task CalculateAsync_WithValidSeatTariff_CreatesTariffItems()
        {
            // Arrange
            var db = CreateInMemoryDb(nameof(CalculateAsync_WithValidSeatTariff_CreatesTariffItems));
            var service = new SeatTariffService(db);

            // Create test data
            var route = new Route { Id = 1, Name = "Test Route" };
            db.Routes!.Add(route);

            var station1 = new Station { Id = 1, Name = "Station A" };
            var station2 = new Station { Id = 2, Name = "Station B" };
            var station3 = new Station { Id = 3, Name = "Station C" };
            db.Stations!.AddRange(station1, station2, station3);

            var routeStations = new[]
            {
                new RouteStation { Id = 1, RouteId = route.Id, StationId = station1.Id, Order = 1, Distance = 0 },
                new RouteStation { Id = 2, RouteId = route.Id, StationId = station2.Id, Order = 2, Distance = 50 },
                new RouteStation { Id = 3, RouteId = route.Id, StationId = station3.Id, Order = 3, Distance = 120 }
            };
            db.RouteStations!.AddRange(routeStations);

            var train = new Train { Id = 1, RouteId = route.Id };
            db.Trains!.Add(train);

            var baseFare = new BaseFare { Id = 1, Price = 10.0 };
            db.BaseFares!.Add(baseFare);

            var tariff = new Tariff { Id = 1, IndexCoefficient = 1.5, VAT = 1.2, BaseFareId = baseFare.Id };
            db.Tariffs!.Add(tariff);

            var seatTariff = new SeatTariff 
            { 
                Id = 1, 
                Name = "Test Seat Tariff",
                TrainId = train.Id,
                TariffId = tariff.Id
            };
            db.SeatTariffs!.Add(seatTariff);

            await db.SaveChangesAsync();

            // Act
            var result = await service.CalculateAsync(seatTariff.Id);

            // Assert
            result.Should().Be(3); // 3 station pairs: A-B, A-C, B-C

            var items = await db.SeatTariffItems!.ToListAsync();
            items.Should().HaveCount(3);

            // Check A-B pair (distance = 50)
            var abItem = items.FirstOrDefault(i => i.FromId == station1.Id && i.ToId == station2.Id);
            abItem.Should().NotBeNull();
            abItem!.Distance.Should().Be(50);
            abItem.Price.Should().Be(50); // distance (as per current implementation)

            // Check A-C pair (distance = 120)
            var acItem = items.FirstOrDefault(i => i.FromId == station1.Id && i.ToId == station3.Id);
            acItem.Should().NotBeNull();
            acItem!.Distance.Should().Be(120);
            acItem.Price.Should().Be(120);

            // Check B-C pair (distance = 70)
            var bcItem = items.FirstOrDefault(i => i.FromId == station2.Id && i.ToId == station3.Id);
            bcItem.Should().NotBeNull();
            bcItem!.Distance.Should().Be(70);
            bcItem.Price.Should().Be(70);
        }

        [Fact]
        public async Task CalculateAsync_WithExistingItems_UpdatesExistingItems()
        {
            // Arrange
            var db = CreateInMemoryDb(nameof(CalculateAsync_WithExistingItems_UpdatesExistingItems));
            var service = new SeatTariffService(db);

            // Create test data
            var route = new Route { Id = 1, Name = "Test Route" };
            db.Routes!.Add(route);

            var station1 = new Station { Id = 1, Name = "Station A" };
            var station2 = new Station { Id = 2, Name = "Station B" };
            db.Stations!.AddRange(station1, station2);

            var routeStations = new[]
            {
                new RouteStation { Id = 1, RouteId = route.Id, StationId = station1.Id, Order = 1, Distance = 0 },
                new RouteStation { Id = 2, RouteId = route.Id, StationId = station2.Id, Order = 2, Distance = 100 }
            };
            db.RouteStations!.AddRange(routeStations);

            var train = new Train { Id = 1, RouteId = route.Id };
            db.Trains!.Add(train);

            var baseFare = new BaseFare { Id = 1, Price = 10.0 };
            db.BaseFares!.Add(baseFare);

            var tariff = new Tariff { Id = 1, IndexCoefficient = 1.5, VAT = 1.2, BaseFareId = baseFare.Id };
            db.Tariffs!.Add(tariff);

            var seatTariff = new SeatTariff 
            { 
                Id = 1, 
                Name = "Test Seat Tariff",
                TrainId = train.Id,
                TariffId = tariff.Id
            };
            db.SeatTariffs!.Add(seatTariff);

            // Add existing item
            var existingItem = new SeatTariffItem
            {
                Id = 1,
                SeatTariffId = seatTariff.Id,
                FromId = station1.Id,
                ToId = station2.Id,
                Distance = 50, // old distance
                Price = 50
            };
            db.SeatTariffItems!.Add(existingItem);

            await db.SaveChangesAsync();

            // Act
            var result = await service.CalculateAsync(seatTariff.Id);

            // Assert
            result.Should().Be(1);

            var items = await db.SeatTariffItems!.ToListAsync();
            items.Should().HaveCount(1);

            var updatedItem = items.First();
            updatedItem.Distance.Should().Be(100); // updated distance
            updatedItem.Price.Should().Be(100); // updated price
        }

        [Fact]
        public async Task CalculateAsync_WithSameStationPair_SkipsItem()
        {
            // Arrange
            var db = CreateInMemoryDb(nameof(CalculateAsync_WithSameStationPair_SkipsItem));
            var service = new SeatTariffService(db);

            // Create test data with same station appearing twice in route
            var route = new Route { Id = 1, Name = "Test Route" };
            db.Routes!.Add(route);

            var station1 = new Station { Id = 1, Name = "Station A" };
            var station2 = new Station { Id = 2, Name = "Station B" };
            db.Stations!.AddRange(station1, station2);

            var routeStations = new[]
            {
                new RouteStation { Id = 1, RouteId = route.Id, StationId = station1.Id, Order = 1, Distance = 0 },
                new RouteStation { Id = 2, RouteId = route.Id, StationId = station2.Id, Order = 2, Distance = 50 },
                new RouteStation { Id = 3, RouteId = route.Id, StationId = station1.Id, Order = 3, Distance = 100 } // Same station again
            };
            db.RouteStations!.AddRange(routeStations);

            var train = new Train { Id = 1, RouteId = route.Id };
            db.Trains!.Add(train);

            var baseFare = new BaseFare { Id = 1, Price = 10.0 };
            db.BaseFares!.Add(baseFare);

            var tariff = new Tariff { Id = 1, IndexCoefficient = 1.5, VAT = 1.2, BaseFareId = baseFare.Id };
            db.Tariffs!.Add(tariff);

            var seatTariff = new SeatTariff 
            { 
                Id = 1, 
                Name = "Test Seat Tariff",
                TrainId = train.Id,
                TariffId = tariff.Id
            };
            db.SeatTariffs!.Add(seatTariff);

            await db.SaveChangesAsync();

            // Act
            var result = await service.CalculateAsync(seatTariff.Id);

            // Assert
            result.Should().Be(2); // Only 2 pairs: A-B, B-A (A-A should be skipped)

            var items = await db.SeatTariffItems!.ToListAsync();
            items.Should().HaveCount(2);

            // Verify no items with same FromId and ToId
            var sameStationItems = items.Where(i => i.FromId == i.ToId);
            sameStationItems.Should().BeEmpty();
        }

        [Fact]
        public async Task CalculateAsync_WithInvalidSeatTariffId_ThrowsArgumentException()
        {
            // Arrange
            var db = CreateInMemoryDb(nameof(CalculateAsync_WithInvalidSeatTariffId_ThrowsArgumentException));
            var service = new SeatTariffService(db);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => service.CalculateAsync(999));
            exception.Message.Should().Contain("SeatTariff with ID 999 not found");
        }

        [Fact]
        public async Task CalculateAsync_WithInsufficientStations_ThrowsInvalidOperationException()
        {
            // Arrange
            var db = CreateInMemoryDb(nameof(CalculateAsync_WithInsufficientStations_ThrowsInvalidOperationException));
            var service = new SeatTariffService(db);

            var route = new Route { Id = 1, Name = "Test Route" };
            db.Routes!.Add(route);

            var train = new Train { Id = 1, RouteId = route.Id };
            db.Trains!.Add(train);

            var baseFare = new BaseFare { Id = 1, Price = 10.0 };
            db.BaseFares!.Add(baseFare);

            var tariff = new Tariff { Id = 1, IndexCoefficient = 1.5, VAT = 1.2, BaseFareId = baseFare.Id };
            db.Tariffs!.Add(tariff);

            var seatTariff = new SeatTariff 
            { 
                Id = 1, 
                Name = "Test Seat Tariff",
                TrainId = train.Id,
                TariffId = tariff.Id
            };
            db.SeatTariffs!.Add(seatTariff);

            // Only one station in route
            var station1 = new Station { Id = 1, Name = "Station A" };
            db.Stations!.Add(station1);

            var routeStation = new RouteStation { Id = 1, RouteId = route.Id, StationId = station1.Id, Order = 1, Distance = 0 };
            db.RouteStations!.Add(routeStation);

            await db.SaveChangesAsync();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => service.CalculateAsync(seatTariff.Id));
            exception.Message.Should().Contain("Train route must contain at least 2 stations");
        }

        [Fact]
        public async Task CalculateAsync_WithMissingTariff_ThrowsInvalidOperationException()
        {
            // Arrange
            var db = CreateInMemoryDb(nameof(CalculateAsync_WithMissingTariff_ThrowsInvalidOperationException));
            var service = new SeatTariffService(db);

            var route = new Route { Id = 1, Name = "Test Route" };
            db.Routes!.Add(route);

            var station1 = new Station { Id = 1, Name = "Station A" };
            var station2 = new Station { Id = 2, Name = "Station B" };
            db.Stations!.AddRange(station1, station2);

            var routeStations = new[]
            {
                new RouteStation { Id = 1, RouteId = route.Id, StationId = station1.Id, Order = 1, Distance = 0 },
                new RouteStation { Id = 2, RouteId = route.Id, StationId = station2.Id, Order = 2, Distance = 50 }
            };
            db.RouteStations!.AddRange(routeStations);

            var train = new Train { Id = 1, RouteId = route.Id };
            db.Trains!.Add(train);

            var seatTariff = new SeatTariff 
            { 
                Id = 1, 
                Name = "Test Seat Tariff",
                TrainId = train.Id,
                TariffId = null // No tariff
            };
            db.SeatTariffs!.Add(seatTariff);

            await db.SaveChangesAsync();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => service.CalculateAsync(seatTariff.Id));
            exception.Message.Should().Contain("SeatTariff must have Tariff with BaseFare");
        }

        [Fact]
        public async Task CalculateAsync_WithMissingBaseFare_ThrowsInvalidOperationException()
        {
            // Arrange
            var db = CreateInMemoryDb(nameof(CalculateAsync_WithMissingBaseFare_ThrowsInvalidOperationException));
            var service = new SeatTariffService(db);

            var route = new Route { Id = 1, Name = "Test Route" };
            db.Routes!.Add(route);

            var station1 = new Station { Id = 1, Name = "Station A" };
            var station2 = new Station { Id = 2, Name = "Station B" };
            db.Stations!.AddRange(station1, station2);

            var routeStations = new[]
            {
                new RouteStation { Id = 1, RouteId = route.Id, StationId = station1.Id, Order = 1, Distance = 0 },
                new RouteStation { Id = 2, RouteId = route.Id, StationId = station2.Id, Order = 2, Distance = 50 }
            };
            db.RouteStations!.AddRange(routeStations);

            var train = new Train { Id = 1, RouteId = route.Id };
            db.Trains!.Add(train);

            var tariff = new Tariff { Id = 1, IndexCoefficient = 1.5, VAT = 1.2, BaseFareId = null }; // No BaseFare
            db.Tariffs!.Add(tariff);

            var seatTariff = new SeatTariff 
            { 
                Id = 1, 
                Name = "Test Seat Tariff",
                TrainId = train.Id,
                TariffId = tariff.Id
            };
            db.SeatTariffs!.Add(seatTariff);

            await db.SaveChangesAsync();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => service.CalculateAsync(seatTariff.Id));
            exception.Message.Should().Contain("SeatTariff must have Tariff with BaseFare");
        }

        [Fact]
        public async Task CalculateAsync_WithZeroDistances_UsesOrderBasedCalculation()
        {
            // Arrange
            var db = CreateInMemoryDb(nameof(CalculateAsync_WithZeroDistances_UsesOrderBasedCalculation));
            var service = new SeatTariffService(db);

            var route = new Route { Id = 1, Name = "Test Route" };
            db.Routes!.Add(route);

            var station1 = new Station { Id = 1, Name = "Station A" };
            var station2 = new Station { Id = 2, Name = "Station B" };
            var station3 = new Station { Id = 3, Name = "Station C" };
            db.Stations!.AddRange(station1, station2, station3);

            // Stations with no distance data (negative values indicate no data)
            var routeStations = new[]
            {
                new RouteStation { Id = 1, RouteId = route.Id, StationId = station1.Id, Order = 1, Distance = -1 },
                new RouteStation { Id = 2, RouteId = route.Id, StationId = station2.Id, Order = 2, Distance = -1 },
                new RouteStation { Id = 3, RouteId = route.Id, StationId = station3.Id, Order = 3, Distance = -1 }
            };
            db.RouteStations!.AddRange(routeStations);

            var train = new Train { Id = 1, RouteId = route.Id };
            db.Trains!.Add(train);

            var baseFare = new BaseFare { Id = 1, Price = 10.0 };
            db.BaseFares!.Add(baseFare);

            var tariff = new Tariff { Id = 1, IndexCoefficient = 1.5, VAT = 1.2, BaseFareId = baseFare.Id };
            db.Tariffs!.Add(tariff);

            var seatTariff = new SeatTariff 
            { 
                Id = 1, 
                Name = "Test Seat Tariff",
                TrainId = train.Id,
                TariffId = tariff.Id
            };
            db.SeatTariffs!.Add(seatTariff);

            await db.SaveChangesAsync();

            // Act
            var result = await service.CalculateAsync(seatTariff.Id);

            // Assert
            result.Should().Be(3);

            var items = await db.SeatTariffItems!.ToListAsync();

            // Check A-B pair (order difference = 1, distance = 1 * 10 = 10)
            var abItem = items.FirstOrDefault(i => i.FromId == station1.Id && i.ToId == station2.Id);
            abItem.Should().NotBeNull();
            abItem!.Distance.Should().Be(10);

            // Check A-C pair (order difference = 2, distance = 2 * 10 = 20)
            var acItem = items.FirstOrDefault(i => i.FromId == station1.Id && i.ToId == station3.Id);
            acItem.Should().NotBeNull();
            acItem!.Distance.Should().Be(20);

            // Check B-C pair (order difference = 1, distance = 1 * 10 = 10)
            var bcItem = items.FirstOrDefault(i => i.FromId == station2.Id && i.ToId == station3.Id);
            bcItem.Should().NotBeNull();
            bcItem!.Distance.Should().Be(10);
        }
    }
}
