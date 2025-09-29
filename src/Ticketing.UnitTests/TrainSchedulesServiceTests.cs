using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.AspNetCore.Exceptions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Models.Dtos;
using Ticketing.Services;
using Xunit;

namespace Ticketing.UnitTests
{
    public class TrainSchedulesServiceTests
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
        public async Task Activate_CreatesSeatsAndSegments()
        {
            var db = CreateInMemoryDb(nameof(Activate_CreatesSeatsAndSegments));

            // Train and schedule
            var train = new Train { Id = 1, RouteId = 10 };
            db.Trains!.Add(train);
            var schedule = new TrainSchedule { Id = 100, TrainId = train.Id, Active = false };
            db.TrainSchedules!.Add(schedule);

            // Route stations 3 hops -> 2 pairs
            db.RouteStations!.AddRange(
                new RouteStation { Id = 1001, RouteId = train.RouteId, Order = 1 },
                new RouteStation { Id = 1002, RouteId = train.RouteId, Order = 2 },
                new RouteStation { Id = 1003, RouteId = train.RouteId, Order = 3 }
            );

            // Wagon and train-wagon (capacity 2 seats)
            var wagon = new Wagon { Id = 2001, SeatCount = 2 };
            db.Wagons!.Add(wagon);
            var trainWagon = new TrainWagon { Id = 3001, TrainScheduleId = schedule.Id, WagonId = wagon.Id };
            db.TrainWagons!.Add(trainWagon);
            await db.SaveChangesAsync();

            var service = new TrainSchedulesService(new NullLogger<TrainSchedulesService>(), db);
            var resp = await service.Activate(schedule.Id);

            resp.ScheduleId.Should().Be(schedule.Id);
            resp.Wagons.Should().Be(1);
            resp.SeatsCreated.Should().Be(2);
            resp.StationPairs.Should().Be(2);
            resp.SegmentsCreated.Should().Be(2 * 2);

            // Seats created
            var seats = await db.Seats!.Where(s => s.WagonId == trainWagon.Id).OrderBy(s => s.Number).ToListAsync();
            seats.Count.Should().Be(2);
            seats.Select(s => s.Number).Should().Contain(new[] { "1", "2" });

            // Segments created: for each seat, two hops
            var segments = await db.SeatSegments!
                .Where(s => s.TrainScheduleId == schedule.Id && s.WagonId == trainWagon.Id)
                .ToListAsync();
            segments.Count.Should().Be(4);
            var pairs = segments.Select(s => (s.FromId, s.ToId)).Distinct().ToList();
            pairs.Should().Contain((1001, 1002));
            pairs.Should().Contain((1002, 1003));
        }

        [Fact]
        public async Task Activate_DoesNotDuplicate_WhenAlreadyCreated()
        {
            var db = CreateInMemoryDb(nameof(Activate_DoesNotDuplicate_WhenAlreadyCreated));

            var train = new Train { Id = 2, RouteId = 20 };
            db.Trains!.Add(train);
            var schedule = new TrainSchedule { Id = 200, TrainId = train.Id, Active = false };
            db.TrainSchedules!.Add(schedule);
            db.RouteStations!.AddRange(
                new RouteStation { Id = 2001, RouteId = train.RouteId, Order = 1 },
                new RouteStation { Id = 2002, RouteId = train.RouteId, Order = 2 }
            );
            var wagon = new Wagon { Id = 2201, SeatCount = 1 };
            db.Wagons!.Add(wagon);
            var trainWagon = new TrainWagon { Id = 2301, TrainScheduleId = schedule.Id, WagonId = wagon.Id };
            db.TrainWagons!.Add(trainWagon);
            await db.SaveChangesAsync();

            // Pre-create seat and segment
            var seat = new Seat { Number = "1", WagonId = trainWagon.Id };
            db.Seats!.Add(seat);
            await db.SaveChangesAsync();
            db.SeatSegments!.Add(new SeatSegment
            {
                SeatId = seat.Id,
                FromId = 2001,
                ToId = 2002,
                TrainId = schedule.TrainId,
                WagonId = trainWagon.Id,
                TrainScheduleId = schedule.Id,
                Price = 0
            });
            await db.SaveChangesAsync();

            var service = new TrainSchedulesService(new NullLogger<TrainSchedulesService>(), db);
            var resp = await service.Activate(schedule.Id);

            resp.SeatsCreated.Should().Be(0);
            resp.SegmentsCreated.Should().Be(0);
        }

        [Fact]
        public async Task Activate_Fails_WhenNoRoute()
        {
            var db = CreateInMemoryDb(nameof(Activate_Fails_WhenNoRoute));

            var train = new Train { Id = 3, RouteId = null };
            db.Trains!.Add(train);
            var schedule = new TrainSchedule { Id = 300, TrainId = train.Id, Active = false };
            db.TrainSchedules!.Add(schedule);
            await db.SaveChangesAsync();

            var service = new TrainSchedulesService(new NullLogger<TrainSchedulesService>(), db);
             var act = () => service.Activate(schedule.Id);
             await act.Should().ThrowAsync<BadRequestException>();
         }

         [Fact]
         public async Task Activate_SetsCorrectDepartureTimes()
         {
             var db = CreateInMemoryDb(nameof(Activate_SetsCorrectDepartureTimes));

             // Train and schedule with specific date
             var train = new Train { Id = 4, RouteId = 40 };
             db.Trains!.Add(train);
             var schedule = new TrainSchedule { Id = 400, TrainId = train.Id, Date = new DateTime(2024, 3, 15, 0, 0, 0, DateTimeKind.Utc), Active = false };
             db.TrainSchedules!.Add(schedule);

             // Route stations with departure times (PostgreSQL style: 1900 base)
             db.RouteStations!.AddRange(
                 new RouteStation { Id = 4001, RouteId = train.RouteId, Order = 1, Departure = new DateTime(1900, 1, 1, 8, 0, 0, DateTimeKind.Utc) }, // 08:00
                 new RouteStation { Id = 4002, RouteId = train.RouteId, Order = 2, Departure = new DateTime(1900, 1, 2, 14, 30, 0, DateTimeKind.Utc) }, // next day 14:30
                 new RouteStation { Id = 4003, RouteId = train.RouteId, Order = 3, Departure = new DateTime(1900, 1, 1, 16, 45, 0, DateTimeKind.Utc) }  // 16:45
             );

             // Wagon and train-wagon
             var wagon = new Wagon { Id = 4001, SeatCount = 1 };
             db.Wagons!.Add(wagon);
             var trainWagon = new TrainWagon { Id = 4001, TrainScheduleId = schedule.Id, WagonId = wagon.Id };
             db.TrainWagons!.Add(trainWagon);
             await db.SaveChangesAsync();

             var service = new TrainSchedulesService(new NullLogger<TrainSchedulesService>(), db);
             await service.Activate(schedule.Id);

             // Check that seats were created first
             var seats = await db.Seats!.Where(s => s.WagonId == trainWagon.Id).ToListAsync();
             seats.Should().NotBeEmpty("Seats should be created");

             // Check departure times
             var segments = await db.SeatSegments!
                 .Where(s => s.TrainScheduleId == schedule.Id)
                 .ToListAsync();

             segments.Should().NotBeEmpty("Segments should be created");
             
             // Debug: show what segments were created
             var fromIds = segments.Select(s => s.FromId).Distinct().ToList();
             fromIds.Should().Contain(4001, "Should contain segment from station 4001");
             fromIds.Should().Contain(4002, "Should contain segment from station 4002");
             fromIds.Should().NotContain(4003, "Station 4003 is final destination, not departure");

             var scheduleDate = schedule.Date.Date; // 2024-03-15

             // Segment pair 1: 4001 -> 4002 (08:00)
             var segment1 = segments.First(s => s.FromId == 4001 && s.ToId == 4002);
             segment1.Departure.Should().Be(scheduleDate.AddHours(8), "Departure time should be 08:00 from station 4001");

             // Segment pair 2: 4002 -> 4003 (next day 14:30)
             var segment2 = segments.First(s => s.FromId == 4002 && s.ToId == 4003);
             segment2.Departure.Should().Be(scheduleDate.AddDays(1).AddHours(14).AddMinutes(30), "Departure time should be next day 14:30 from station 4002");
         }

         [Fact]
         public async Task CreateSchedulesByDatesAsync_SetsCorrectDepartureTimes()
         {
             var db = CreateInMemoryDb(nameof(CreateSchedulesByDatesAsync_SetsCorrectDepartureTimes));

             // Wagon first
             var wagon = new Wagon { Id = 5001, SeatCount = 2 };
             db.Wagons!.Add(wagon);

             // Train plan first
             var plan = new TrainWagonsPlan { Id = 5001, TrainId = 5 };
             db.TrainWagonsPlans!.Add(plan);

             var planWagon = new TrainWagonsPlanWagon { Id = 5001, PlanId = plan.Id, WagonId = wagon.Id, Number = "1" };
             db.TrainWagonsPlanWagons!.Add(planWagon);

             // Train and route (after plan is created)
             var train = new Train { Id = 5, RouteId = 50, PlanId = plan.Id };
             db.Trains!.Add(train);

             // Route stations with departure times (PostgreSQL style: 1900 base)
             db.RouteStations!.AddRange(
                 new RouteStation { Id = 5001, RouteId = train.RouteId, Order = 1, Departure = new DateTime(1900, 1, 1, 9, 15, 0, DateTimeKind.Utc) }, // 09:15
                 new RouteStation { Id = 5002, RouteId = train.RouteId, Order = 2, Departure = new DateTime(1900, 1, 2, 11, 45, 0, DateTimeKind.Utc) }, // next day 11:45
                 new RouteStation { Id = 5003, RouteId = train.RouteId, Order = 3, Departure = new DateTime(1900, 1, 1, 17, 30, 0, DateTimeKind.Utc) }  // 17:30
             );

             await db.SaveChangesAsync();

             var service = new TrainSchedulesService(new NullLogger<TrainSchedulesService>(), db);

             var request = new TrainScheduleDatesRequestDto
             {
                 TrainId = train.Id,
                 Dates = new List<DateTime> { new DateTime(2024, 4, 20, 0, 0, 0, DateTimeKind.Utc) }
             };

             // Act
             var result = await service.CreateSchedulesByDatesAsync(request);

             // Assert
             result.SchedulesCreated.Should().Be(1);
             result.SeatSegmentsCreated.Should().BeGreaterThan(0);

             // Check that seats were created
             var trainWagon = await db.TrainWagons!.FirstAsync(tw => tw.TrainScheduleId.HasValue);
             var seats = await db.Seats!.Where(s => s.WagonId == trainWagon.Id).ToListAsync();
             seats.Should().NotBeEmpty("Seats should be created");

             // Check departure times
             var segments = await db.SeatSegments!
                 .Where(s => s.TrainScheduleId.HasValue)
                 .ToListAsync();

             segments.Should().NotBeEmpty("Segments should be created");

             // Debug: show what segments were created
             var fromIds = segments.Select(s => s.FromId).Distinct().ToList();
             fromIds.Should().Contain(5001, "Should contain segment from station 5001");
             fromIds.Should().Contain(5002, "Should contain segment from station 5002");
             fromIds.Should().NotContain(5003, "Station 5003 is final destination, not departure");

             var scheduleDate = request.Dates[0].Date; // 2024-04-20

             // Segment pair 1: 5001 -> 5002 (09:15)
             var segment1 = segments.First(s => s.FromId == 5001 && s.ToId == 5002);
             segment1.Departure.Should().Be(scheduleDate.AddHours(9).AddMinutes(15), "Departure time should be 09:15 from station 5001");

             // Segment pair 2: 5002 -> 5003 (next day 11:45)
             var segment2 = segments.First(s => s.FromId == 5002 && s.ToId == 5003);
             segment2.Departure.Should().Be(scheduleDate.AddDays(1).AddHours(11).AddMinutes(45), "Departure time should be next day 11:45 from station 5002");
         }
     }
 }


