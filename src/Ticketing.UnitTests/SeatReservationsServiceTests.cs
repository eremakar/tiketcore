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
using Ticketing.Services;
using Ticketing.Models.Dtos;
using Xunit;

namespace Ticketing.UnitTests
{
    public class SeatReservationsServiceTests
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
        public async Task Reserve_Success_MarksAllHops_AndCreatesReservation()
        {
            var db = CreateInMemoryDb(nameof(Reserve_Success_MarksAllHops_AndCreatesReservation));

            var trainScheduleId = 100L;
            // seed train, schedule, route stations (1-2-3)
            await SeedTrainWithScheduleAndRouteAsync(db, trainScheduleId, (1, 1), (2, 2), (3, 3));
            var trainWagonId = await SeedTrainWagonWithCapacityAsync(db, trainScheduleId, seatCapacity: 2);

            // activate to generate seats and segments
            var tsService = new TrainSchedulesService(new NullLogger<TrainSchedulesService>(), db);
            await tsService.Activate(trainScheduleId);

            // pick first seat in wagon
            var seatId = await db.Seats!.Where(s => s.WagonId == trainWagonId).Select(s => s.Id).FirstAsync();

            var service = new SeatReservationsService(new NullLogger<SeatReservationsService>(), db);
            var resp = await service.Reserve(trainScheduleId, trainWagonId, seatId, 1, 3, price: 100);

            resp.Result.Should().Be(SeatReservationResult.Success);
            resp.ReservationId.Should().NotBeNull();

            // Verify segments for that seat are marked
            var segments = await db.SeatSegments!
                .Where(s => s.TrainScheduleId == trainScheduleId && s.WagonId == trainWagonId && s.SeatId == seatId)
                .OrderBy(s => s.FromId)
                .ToListAsync();
            segments.Count.Should().Be(2);
            segments.All(s => s.SeatReservationId == resp.ReservationId).Should().BeTrue();
        }

        [Fact]
        public async Task Reserve_Fails_WhenIncompleteSegments()
        {
            var db = CreateInMemoryDb(nameof(Reserve_Fails_WhenIncompleteSegments));
            var trainScheduleId = 101L;
            await SeedTrainWithScheduleAndRouteAsync(db, trainScheduleId, (1, 1), (2, 2), (3, 3));
            var trainWagonId = await SeedTrainWagonWithCapacityAsync(db, trainScheduleId, seatCapacity: 2);
            var tsService = new TrainSchedulesService(new NullLogger<TrainSchedulesService>(), db);
            await tsService.Activate(trainScheduleId);
            var seatId = await db.Seats!.Where(s => s.WagonId == trainWagonId).Select(s => s.Id).FirstAsync();

            // remove one hop (2-3) for that seat to simulate incomplete
            var seg23 = await db.SeatSegments!.FirstAsync(s => s.TrainScheduleId == trainScheduleId && s.WagonId == trainWagonId && s.SeatId == seatId && s.FromId == 2 && s.ToId == 3);
            db.SeatSegments!.Remove(seg23);
            await db.SaveChangesAsync();

            var service = new SeatReservationsService(new NullLogger<SeatReservationsService>(), db);
            var resp = await service.Reserve(trainScheduleId, trainWagonId, seatId, 1, 3);

            resp.Result.Should().Be(SeatReservationResult.IncompleteSegments);
            resp.ReservationId.Should().BeNull();
        }

        [Fact]
        public async Task Reserve_Fails_WhenSegmentsOccupied()
        {
            var db = CreateInMemoryDb(nameof(Reserve_Fails_WhenSegmentsOccupied));
            var trainScheduleId = 102L;
            await SeedTrainWithScheduleAndRouteAsync(db, trainScheduleId, (1, 1), (2, 2), (3, 3));
            var trainWagonId = await SeedTrainWagonWithCapacityAsync(db, trainScheduleId, seatCapacity: 2);
            var tsService = new TrainSchedulesService(new NullLogger<TrainSchedulesService>(), db);
            await tsService.Activate(trainScheduleId);
            var seatId = await db.Seats!.Where(s => s.WagonId == trainWagonId).Select(s => s.Id).FirstAsync();

            // Occupy one hop for that seat
            var segToOccupy = await db.SeatSegments!.FirstAsync(s => s.TrainScheduleId == trainScheduleId && s.WagonId == trainWagonId && s.SeatId == seatId && s.FromId == 2 && s.ToId == 3);
            segToOccupy.SeatReservationId = 9999; // mark occupied
            await db.SaveChangesAsync();

            var service = new SeatReservationsService(new NullLogger<SeatReservationsService>(), db);
            var resp = await service.Reserve(trainScheduleId, trainWagonId, seatId, 1, 3);

            resp.Result.Should().Be(SeatReservationResult.SegmentsOccupied);
            resp.ReservationId.Should().BeNull();
        }

        [Fact]
        public async Task Reserve_Fails_ValidationFailed_WhenCapacityExceeded()
        {
            var db = CreateInMemoryDb(nameof(Reserve_Fails_ValidationFailed_WhenCapacityExceeded));
            var trainScheduleId = 103L;
            await SeedTrainWithScheduleAndRouteAsync(db, trainScheduleId, (1, 1), (2, 2), (3, 3));
            var trainWagonId = await SeedTrainWagonWithCapacityAsync(db, trainScheduleId, seatCapacity: 1);
            var tsService = new TrainSchedulesService(new NullLogger<TrainSchedulesService>(), db);
            await tsService.Activate(trainScheduleId);
            var seatId = await db.Seats!.Where(s => s.WagonId == trainWagonId).Select(s => s.Id).FirstAsync();

            // Existing count segment overlaps with requested [1,3] with large count
            await SeedSeatCountSegmentAsync(db, seatCount: 3, trainScheduleId: trainScheduleId, wagonId: trainWagonId, fromId: 1, toId: 3);

            var service = new SeatReservationsService(new NullLogger<SeatReservationsService>(), db);
            var resp = await service.Reserve(trainScheduleId, trainWagonId, seatId, 1, 3);

            resp.Result.Should().Be(SeatReservationResult.ValidationFailed);
            resp.ReservationId.Should().BeNull();
            resp.Message.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Reserve_InvalidInput_WhenOrdersMissing()
        {
            var db = CreateInMemoryDb(nameof(Reserve_InvalidInput_WhenOrdersMissing));
            var trainScheduleId = 104L;
            // seed route for different ids (10,20)
            await SeedTrainWithScheduleAndRouteAsync(db, trainScheduleId, (10, 1), (20, 2));
            var trainWagonId = await SeedTrainWagonWithCapacityAsync(db, trainScheduleId, seatCapacity: 1);
            var tsService = new TrainSchedulesService(new NullLogger<TrainSchedulesService>(), db);
            await tsService.Activate(trainScheduleId);
            var seatId = await db.Seats!.Where(s => s.WagonId == trainWagonId).Select(s => s.Id).FirstAsync();

            var service = new SeatReservationsService(new NullLogger<SeatReservationsService>(), db);
            var resp = await service.Reserve(trainScheduleId, trainWagonId, seatId, fromRouteStationId: 1, toRouteStationId: 2);

            resp.Result.Should().Be(SeatReservationResult.InvalidInput);
        }

        [Fact]
        public async Task Reserve_NotFound_WhenNoSegments()
        {
            var db = CreateInMemoryDb(nameof(Reserve_NotFound_WhenNoSegments));
            var trainScheduleId = 105L;
            // create route that doesn't match fake seatId
            await SeedTrainWithScheduleAndRouteAsync(db, trainScheduleId, (1, 1), (2, 2));
            var trainWagonId = await SeedTrainWagonWithCapacityAsync(db, trainScheduleId, seatCapacity: 1);
            var tsService = new TrainSchedulesService(new NullLogger<TrainSchedulesService>(), db);
            await tsService.Activate(trainScheduleId);
            var fakeSeatId = 999999L; // seat does not exist -> no segments

            var service = new SeatReservationsService(new NullLogger<SeatReservationsService>(), db);
            var resp = await service.Reserve(trainScheduleId, trainWagonId, fakeSeatId, 1, 2);

            resp.Result.Should().Be(SeatReservationResult.NotFound);
            resp.ReservationId.Should().BeNull();
        }
        private static async Task SeedRouteStationsAsync(TicketDbContext db, long routeId, params (long id, int order)[] stations)
        {
            foreach (var s in stations)
                db.RouteStations!.Add(new RouteStation { Id = s.id, Order = s.order, RouteId = routeId });
            await db.SaveChangesAsync();
        }

        private static async Task SeedReservationAsync(TicketDbContext db, long id, long seatId, long trainScheduleId, long fromId, long toId)
        {
            db.SeatReservations!.Add(new SeatReservation
            {
                Id = id,
                SeatId = seatId,
                TrainScheduleId = trainScheduleId,
                FromId = fromId,
                ToId = toId
            });
            await db.SaveChangesAsync();
        }

        private static async Task SeedSeatSegmentAsync(TicketDbContext db, long seatId, long trainScheduleId, long fromId, long toId, long? wagonId = null)
        {
            db.SeatSegments!.Add(new SeatSegment
            {
                SeatId = seatId,
                TrainScheduleId = trainScheduleId,
                FromId = fromId,
                ToId = toId,
                WagonId = wagonId
            });
            await db.SaveChangesAsync();
        }

        private static async Task SeedSeatCountSegmentAsync(TicketDbContext db, int seatCount, long trainScheduleId, long wagonId, long fromId, long toId)
        {
            db.SeatCountSegments!.Add(new SeatCountSegment
            {
                SeatCount = seatCount,
                FreeCount = 0,
                TrainScheduleId = trainScheduleId,
                WagonId = wagonId,
                FromId = fromId,
                ToId = toId
            });
            await db.SaveChangesAsync();
        }

        private static async Task<long> SeedTrainWagonWithCapacityAsync(TicketDbContext db, long trainScheduleId, int seatCapacity)
        {
            var schedule = await db.TrainSchedules!.FindAsync(trainScheduleId);
            if (schedule == null)
            {
                schedule = new TrainSchedule { Id = trainScheduleId, Active = true };
                db.TrainSchedules!.Add(schedule);
            }

            var wagon = new Wagon { Id = 9000 + trainScheduleId, SeatCount = seatCapacity };
            db.Wagons!.Add(wagon);

            var trainWagon = new TrainWagon
            {
                Id = 8000 + trainScheduleId,
                TrainScheduleId = trainScheduleId,
                WagonId = wagon.Id
            };
            db.TrainWagons!.Add(trainWagon);
            await db.SaveChangesAsync();
            return trainWagon.Id;
        }

        private static async Task SeedTrainWithScheduleAndRouteAsync(TicketDbContext db, long trainScheduleId, params (long id, int order)[] stations)
        {
            var train = new Train { Id = 7000 + trainScheduleId, RouteId = 6000 + trainScheduleId };
            db.Trains!.Add(train);
            var schedule = await db.TrainSchedules!.FindAsync(trainScheduleId);
            if (schedule == null)
            {
                schedule = new TrainSchedule { Id = trainScheduleId, Active = false };
                db.TrainSchedules!.Add(schedule);
            }
            schedule.TrainId = train.Id;
            await db.SaveChangesAsync();

            await SeedRouteStationsAsync(db, train.RouteId!.Value, stations);
        }

        [Fact]
        public async Task ValidateReservation_NoExisting_ShouldPass()
        {
            var db = CreateInMemoryDb(nameof(ValidateReservation_NoExisting_ShouldPass));
            await SeedRouteStationsAsync(db, 1L, (1, 1), (2, 2));

            var service = new SeatReservationsService(new NullLogger<SeatReservationsService>(), db);
            var candidate = new SeatReservation { Id = 10, SeatId = 100, TrainScheduleId = 1, FromId = 1, ToId = 2 };

            await service.ValidateReservation(candidate);
        }

        [Fact]
        public async Task ValidateReservation_SameSeat_SameTrainSchedule_Overlap_ShouldThrow()
        {
            var db = CreateInMemoryDb(nameof(ValidateReservation_SameSeat_SameTrainSchedule_Overlap_ShouldThrow));
            await SeedRouteStationsAsync(db, 1L, (1, 1), (2, 2), (3, 3));
            await SeedSeatSegmentAsync(db, seatId: 100, trainScheduleId: 1000, fromId: 1, toId: 3);
            // Mark the existing segment as occupied so overlap detection applies
            var existing = await db.SeatSegments!.FirstAsync(s => s.SeatId == 100 && s.TrainScheduleId == 1000 && s.FromId == 1 && s.ToId == 3);
            existing.SeatReservationId = 7777;
            await db.SaveChangesAsync();

            var service = new SeatReservationsService(new NullLogger<SeatReservationsService>(), db);
            var candidate = new SeatReservation { Id = 10, SeatId = 100, TrainScheduleId = 1000, FromId = 2, ToId = 3 };

            Func<Task> act = () => service.ValidateReservation(candidate);
            await act.Should().ThrowAsync<BadRequestException>();
        }

        [Fact]
        public async Task ValidateReservation_SameSeat_SameTrainSchedule_TouchingEndpoint_ShouldPass()
        {
            var db = CreateInMemoryDb(nameof(ValidateReservation_SameSeat_SameTrainSchedule_TouchingEndpoint_ShouldPass));
            await SeedRouteStationsAsync(db, 1L, (1, 1), (2, 2), (3, 3), (4, 4));
            await SeedSeatSegmentAsync(db, seatId: 100, trainScheduleId: 2000, fromId: 1, toId: 2);

            var service = new SeatReservationsService(new NullLogger<SeatReservationsService>(), db);
            var candidate = new SeatReservation { Id = 10, SeatId = 100, TrainScheduleId = 2000, FromId = 2, ToId = 4 };

            await service.ValidateReservation(candidate);
        }

        [Fact]
        public async Task ValidateReservation_DifferentSeat_ShouldPass()
        {
            var db = CreateInMemoryDb(nameof(ValidateReservation_DifferentSeat_ShouldPass));
            await SeedRouteStationsAsync(db, 1L, (1, 1), (2, 2), (3, 3));
            await SeedSeatSegmentAsync(db, seatId: 100, trainScheduleId: 3000, fromId: 1, toId: 3);

            var service = new SeatReservationsService(new NullLogger<SeatReservationsService>(), db);
            var candidate = new SeatReservation { Id = 10, SeatId = 101, TrainScheduleId = 3000, FromId = 1, ToId = 2 };

            await service.ValidateReservation(candidate);
        }

        [Fact]
        public async Task ValidateReservation_DifferentTrainSchedule_ShouldPass()
        {
            var db = CreateInMemoryDb(nameof(ValidateReservation_DifferentTrainSchedule_ShouldPass));
            await SeedRouteStationsAsync(db, 1L, (1, 1), (2, 2), (3, 3));
            await SeedSeatSegmentAsync(db, seatId: 100, trainScheduleId: 4000, fromId: 1, toId: 3);

            var service = new SeatReservationsService(new NullLogger<SeatReservationsService>(), db);
            var candidate = new SeatReservation { Id = 10, SeatId = 100, TrainScheduleId = 4001, FromId = 1, ToId = 2 };

            await service.ValidateReservation(candidate);
        }

        [Fact]
        public async Task ValidateReservation_NullFromOrTo_ShouldPass()
        {
            var db = CreateInMemoryDb(nameof(ValidateReservation_NullFromOrTo_ShouldPass));
            var service = new SeatReservationsService(new NullLogger<SeatReservationsService>(), db);
            var candidate = new SeatReservation { Id = 10, SeatId = 100, TrainScheduleId = 5555, FromId = null, ToId = 2 };
            await service.ValidateReservation(candidate);
        }

        [Fact]
        public async Task ValidateCountReservation_WithinCapacity_WithExistingCountSegments_ShouldPass()
        {
            var db = CreateInMemoryDb(nameof(ValidateCountReservation_WithinCapacity_WithExistingCountSegments_ShouldPass));
            await SeedRouteStationsAsync(db, 1L, (1, 1), (2, 2), (3, 3));
            var trainScheduleId = 10L;
            var trainWagonId = await SeedTrainWagonWithCapacityAsync(db, trainScheduleId, seatCapacity: 3);
            // one existing count segment overlapping [1,3] with 2 seats
            await SeedSeatCountSegmentAsync(db, seatCount: 2, trainScheduleId: trainScheduleId, wagonId: trainWagonId, fromId: 1, toId: 3);

            var service = new SeatReservationsService(new NullLogger<SeatReservationsService>(), db);
            var candidate = new SeatCountReservation { Id = 2, TrainScheduleId = trainScheduleId, WagonId = trainWagonId, FromId = 2, ToId = 3, SeatCount = 2 };

            await service.ValidateReservation(candidate);
        }

        [Fact]
        public async Task ValidateCountReservation_ExceedsCapacity_WithExistingCountSegments_ShouldThrow()
        {
            var db = CreateInMemoryDb(nameof(ValidateCountReservation_ExceedsCapacity_WithExistingCountSegments_ShouldThrow));
            await SeedRouteStationsAsync(db, 1L, (1, 1), (2, 2), (3, 3));
            var trainScheduleId = 11L;
            var trainWagonId = await SeedTrainWagonWithCapacityAsync(db, trainScheduleId, seatCapacity: 2);
            // existing count segment overlapping [1,3] with 5 seats
            await SeedSeatCountSegmentAsync(db, seatCount: 5, trainScheduleId: trainScheduleId, wagonId: trainWagonId, fromId: 1, toId: 3);

            var service = new SeatReservationsService(new NullLogger<SeatReservationsService>(), db);
            var candidate = new SeatCountReservation { Id = 2, TrainScheduleId = trainScheduleId, WagonId = trainWagonId, FromId = 2, ToId = 3, SeatCount = 2 };

            Func<Task> act = () => service.ValidateReservation(candidate);
            await act.Should().ThrowAsync<BadRequestException>();
        }

        [Fact]
        public async Task ValidateCountReservation_TouchingEndpoints_ShouldPass_WithOverlapRule()
        {
            var db = CreateInMemoryDb(nameof(ValidateCountReservation_TouchingEndpoints_ShouldPass_WithOverlapRule));
            await SeedRouteStationsAsync(db, 1L, (1, 1), (2, 2), (3, 3));
            var trainScheduleId = 12L;
            var trainWagonId = await SeedTrainWagonWithCapacityAsync(db, trainScheduleId, seatCapacity: 1);

            // existing count segment on [1,2]
            await SeedSeatCountSegmentAsync(db, seatCount: 1, trainScheduleId: trainScheduleId, wagonId: trainWagonId, fromId: 1, toId: 2);

            var service = new SeatReservationsService(new NullLogger<SeatReservationsService>(), db);
            var candidate = new SeatCountReservation { Id = 2, TrainScheduleId = trainScheduleId, WagonId = trainWagonId, FromId = 2, ToId = 3, SeatCount = 1 };

            await service.ValidateReservation(candidate);
        }

        [Fact]
        public async Task ValidateSingleSeat_ShouldRespectExistingCountSegments()
        {
            var db = CreateInMemoryDb(nameof(ValidateSingleSeat_ShouldRespectExistingCountSegments));
            await SeedRouteStationsAsync(db, 1L, (1, 1), (2, 2), (3, 3));
            var trainScheduleId = 13L;
            var trainWagonId = await SeedTrainWagonWithCapacityAsync(db, trainScheduleId, seatCapacity: 1);

            // existing count segment occupies 3 on [1,3]
            await SeedSeatCountSegmentAsync(db, seatCount: 3, trainScheduleId: trainScheduleId, wagonId: trainWagonId, fromId: 1, toId: 3);

            var service = new SeatReservationsService(new NullLogger<SeatReservationsService>(), db);
            var candidate = new SeatReservation { Id = 2, TrainScheduleId = trainScheduleId, WagonId = trainWagonId, FromId = 2, ToId = 3, SeatId = 100 };

            Func<Task> act = () => service.ValidateReservation(candidate);
            await act.Should().ThrowAsync<BadRequestException>();
        }
    }
}


