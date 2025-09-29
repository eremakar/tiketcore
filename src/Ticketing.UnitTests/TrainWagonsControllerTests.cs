using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Services;
using Xunit;

namespace Ticketing.UnitTests
{
    public class TrainWagonsServiceTests
    {
        private static TicketDbContext CreateInMemoryDb(string dbName)
        {
            var options = new DbContextOptionsBuilder<TicketDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            var ctx = new TicketDbContext(options);
            return ctx;
        }

        private static TrainWagonsService CreateService(TicketDbContext db)
        {
            var logger = new NullLogger<TrainWagonsService>();
            return new TrainWagonsService(db, logger);
        }

        [Fact]
        public async Task GenerateSeatsAsync_Success_GeneratesCorrectNumberOfSeats()
        {
            // Arrange
            var db = CreateInMemoryDb(nameof(GenerateSeatsAsync_Success_GeneratesCorrectNumberOfSeats));
            var service = CreateService(db);

            // Create test data
            var wagon = new Wagon { Id = 1, SeatCount = 5, Type = new WagonType { Name = "TestWagon" }, Class = "Economy" };
            db.Wagons!.Add(wagon);
            
            var trainSchedule = new TrainSchedule { Id = 1, Active = true };
            db.TrainSchedules!.Add(trainSchedule);
            
            var trainWagon = new TrainWagon { Id = 1, WagonId = wagon.Id, TrainScheduleId = trainSchedule.Id };
            db.TrainWagons!.Add(trainWagon);
            await db.SaveChangesAsync();

            // Act
            var result = await service.GenerateSeatsAsync(1);

            // Assert
            result.Should().Be(5);

            // Verify seats were created in database
            var seats = await db.Seats!.Where(s => s.WagonId == 1).OrderBy(s => s.Number).ToListAsync();
            seats.Count.Should().Be(5);
            seats.Select(s => s.Number).Should().Contain(new[] { "1", "2", "3", "4", "5" });
            seats.All(s => s.Class == 0).Should().BeTrue();
            seats.All(s => s.TypeId == null).Should().BeTrue();
        }

        [Fact]
        public async Task GenerateSeatsAsync_TrainWagonNotFound_ReturnsNotFound()
        {
            // Arrange
            var db = CreateInMemoryDb(nameof(GenerateSeatsAsync_TrainWagonNotFound_ReturnsNotFound));
            var service = CreateService(db);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => service.GenerateSeatsAsync(999));
            exception.Message.Should().Be("Train wagon with id 999 not found");
        }

        [Fact]
        public async Task GenerateSeatsAsync_WagonNotFound_ReturnsBadRequest()
        {
            // Arrange
            var db = CreateInMemoryDb(nameof(GenerateSeatsAsync_WagonNotFound_ReturnsBadRequest));
            var service = CreateService(db);

            var trainSchedule = new TrainSchedule { Id = 1, Active = true };
            db.TrainSchedules!.Add(trainSchedule);
            
            var trainWagon = new TrainWagon { Id = 1, WagonId = null, TrainScheduleId = trainSchedule.Id };
            db.TrainWagons!.Add(trainWagon);
            await db.SaveChangesAsync();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => service.GenerateSeatsAsync(1));
            exception.Message.Should().Be("Train wagon has no associated wagon");
        }

        [Fact]
        public async Task GenerateSeatsAsync_ZeroSeatCount_ReturnsBadRequest()
        {
            // Arrange
            var db = CreateInMemoryDb(nameof(GenerateSeatsAsync_ZeroSeatCount_ReturnsBadRequest));
            var service = CreateService(db);

            var wagon = new Wagon { Id = 1, SeatCount = 0, Type = new WagonType { Name = "TestWagon" }, Class = "Economy" };
            db.Wagons!.Add(wagon);
            
            var trainSchedule = new TrainSchedule { Id = 1, Active = true };
            db.TrainSchedules!.Add(trainSchedule);
            
            var trainWagon = new TrainWagon { Id = 1, WagonId = wagon.Id, TrainScheduleId = trainSchedule.Id };
            db.TrainWagons!.Add(trainWagon);
            await db.SaveChangesAsync();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => service.GenerateSeatsAsync(1));
            exception.Message.Should().Be("Wagon seat count must be greater than 0");
        }

        [Fact]
        public async Task GenerateSeatsAsync_WithExistingSeats_SkipsDuplicates()
        {
            // Arrange
            var db = CreateInMemoryDb(nameof(GenerateSeatsAsync_WithExistingSeats_SkipsDuplicates));
            var service = CreateService(db);

            var wagon = new Wagon { Id = 1, SeatCount = 5, Type = new WagonType { Name = "TestWagon" }, Class = "Economy" };
            db.Wagons!.Add(wagon);
            
            var trainSchedule = new TrainSchedule { Id = 1, Active = true };
            db.TrainSchedules!.Add(trainSchedule);
            
            var trainWagon = new TrainWagon { Id = 1, WagonId = wagon.Id, TrainScheduleId = trainSchedule.Id };
            db.TrainWagons!.Add(trainWagon);

            // Pre-create some seats
            db.Seats!.AddRange(
                new Seat { Number = "1", WagonId = 1, Class = 0 },
                new Seat { Number = "3", WagonId = 1, Class = 0 },
                new Seat { Number = "5", WagonId = 1, Class = 0 }
            );
            await db.SaveChangesAsync();

            // Act
            var result = await service.GenerateSeatsAsync(1);

            // Assert
            result.Should().Be(2);

            // Verify all seats exist
            var seats = await db.Seats!.Where(s => s.WagonId == 1).OrderBy(s => s.Number).ToListAsync();
            seats.Count.Should().Be(5);
            seats.Select(s => s.Number).Should().Contain(new[] { "1", "2", "3", "4", "5" });
        }

        [Fact]
        public async Task GenerateSeatsAsync_AllSeatsExist_GeneratesNoNewSeats()
        {
            // Arrange
            var db = CreateInMemoryDb(nameof(GenerateSeatsAsync_AllSeatsExist_GeneratesNoNewSeats));
            var service = CreateService(db);

            var wagon = new Wagon { Id = 1, SeatCount = 3, Type = new WagonType { Name = "TestWagon" }, Class = "Economy" };
            db.Wagons!.Add(wagon);
            
            var trainSchedule = new TrainSchedule { Id = 1, Active = true };
            db.TrainSchedules!.Add(trainSchedule);
            
            var trainWagon = new TrainWagon { Id = 1, WagonId = wagon.Id, TrainScheduleId = trainSchedule.Id };
            db.TrainWagons!.Add(trainWagon);

            // Pre-create all seats
            db.Seats!.AddRange(
                new Seat { Number = "1", WagonId = 1, Class = 0 },
                new Seat { Number = "2", WagonId = 1, Class = 0 },
                new Seat { Number = "3", WagonId = 1, Class = 0 }
            );
            await db.SaveChangesAsync();

            // Act
            var result = await service.GenerateSeatsAsync(1);

            // Assert
            result.Should().Be(0);

            // Verify no additional seats were created
            var seats = await db.Seats!.Where(s => s.WagonId == 1).ToListAsync();
            seats.Count.Should().Be(3);
        }

        [Fact]
        public async Task GenerateSeatsAsync_LargeSeatCount_GeneratesCorrectly()
        {
            // Arrange
            var db = CreateInMemoryDb(nameof(GenerateSeatsAsync_LargeSeatCount_GeneratesCorrectly));
            var service = CreateService(db);

            var wagon = new Wagon { Id = 1, SeatCount = 100, Type = new WagonType { Name = "TestWagon" }, Class = "Economy" };
            db.Wagons!.Add(wagon);
            
            var trainSchedule = new TrainSchedule { Id = 1, Active = true };
            db.TrainSchedules!.Add(trainSchedule);
            
            var trainWagon = new TrainWagon { Id = 1, WagonId = wagon.Id, TrainScheduleId = trainSchedule.Id };
            db.TrainWagons!.Add(trainWagon);
            await db.SaveChangesAsync();

            // Act
            var result = await service.GenerateSeatsAsync(1);

            // Assert
            result.Should().Be(100);

            // Verify seats were created
            var seats = await db.Seats!.Where(s => s.WagonId == 1).ToListAsync();
            seats.Count.Should().Be(100);
            
            // Verify seat numbers are sequential
            var seatNumbers = seats.Select(s => int.Parse(s.Number!)).OrderBy(n => n).ToList();
            seatNumbers.Should().BeEquivalentTo(Enumerable.Range(1, 100));
        }
    }

}
