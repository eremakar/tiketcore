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
using Ticketing.Services.GraphQL;
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

        private static async Task SeedRouteStationsAsync(TicketDbContext db, params (long id, int order)[] stations)
        {
            foreach (var s in stations)
                db.RouteStations!.Add(new RouteStation { Id = s.id, Order = s.order });
            await db.SaveChangesAsync();
        }

        private static async Task SeedReservationAsync(TicketDbContext db, long id, long seatId, DateTime date, long fromId, long toId)
        {
            db.SeatReservations!.Add(new SeatReservation
            {
                Id = id,
                SeatId = seatId,
                Date = date.Date,
                FromId = fromId,
                ToId = toId
            });
            await db.SaveChangesAsync();
        }

        [Fact]
        public async Task ValidateReservation_NoExisting_ShouldPass()
        {
            var db = CreateInMemoryDb(nameof(ValidateReservation_NoExisting_ShouldPass));
            await SeedRouteStationsAsync(db, (1, 1), (2, 2));

            var service = new SeatReservationsService(new NullLogger<SeatReservationsService>(), db);
            var candidate = new SeatReservation { Id = 10, SeatId = 100, Date = DateTime.UtcNow.Date, FromId = 1, ToId = 2 };

            await service.ValidateReservation(candidate);
        }

        [Fact]
        public async Task ValidateReservation_SameSeat_SameDate_Overlap_ShouldThrow()
        {
            var db = CreateInMemoryDb(nameof(ValidateReservation_SameSeat_SameDate_Overlap_ShouldThrow));
            await SeedRouteStationsAsync(db, (1, 1), (2, 2), (3, 3));
            var date = DateTime.UtcNow.Date;
            await SeedReservationAsync(db, 1, seatId: 100, date, fromId: 1, toId: 3);

            var service = new SeatReservationsService(new NullLogger<SeatReservationsService>(), db);
            var candidate = new SeatReservation { Id = 10, SeatId = 100, Date = date, FromId = 2, ToId = 3 };

            Func<Task> act = () => service.ValidateReservation(candidate);
            await act.Should().ThrowAsync<BadRequestException>();
        }

        [Fact]
        public async Task ValidateReservation_SameSeat_SameDate_TouchingEndpoint_ShouldPass()
        {
            var db = CreateInMemoryDb(nameof(ValidateReservation_SameSeat_SameDate_TouchingEndpoint_ShouldPass));
            await SeedRouteStationsAsync(db, (1, 1), (2, 2), (3, 3), (4, 4));
            var date = DateTime.UtcNow.Date;
            await SeedReservationAsync(db, 1, seatId: 100, date, fromId: 1, toId: 2);

            var service = new SeatReservationsService(new NullLogger<SeatReservationsService>(), db);
            var candidate = new SeatReservation { Id = 10, SeatId = 100, Date = date, FromId = 2, ToId = 4 };

            await service.ValidateReservation(candidate);
        }

        [Fact]
        public async Task ValidateReservation_DifferentSeat_ShouldPass()
        {
            var db = CreateInMemoryDb(nameof(ValidateReservation_DifferentSeat_ShouldPass));
            await SeedRouteStationsAsync(db, (1, 1), (2, 2), (3, 3));
            var date = DateTime.UtcNow.Date;
            await SeedReservationAsync(db, 1, seatId: 100, date, fromId: 1, toId: 3);

            var service = new SeatReservationsService(new NullLogger<SeatReservationsService>(), db);
            var candidate = new SeatReservation { Id = 10, SeatId = 101, Date = date, FromId = 1, ToId = 2 };

            await service.ValidateReservation(candidate);
        }

        [Fact]
        public async Task ValidateReservation_DifferentDate_ShouldPass()
        {
            var db = CreateInMemoryDb(nameof(ValidateReservation_DifferentDate_ShouldPass));
            await SeedRouteStationsAsync(db, (1, 1), (2, 2), (3, 3));
            var date = DateTime.UtcNow.Date;
            await SeedReservationAsync(db, 1, seatId: 100, date: date.AddDays(-1), fromId: 1, toId: 3);

            var service = new SeatReservationsService(new NullLogger<SeatReservationsService>(), db);
            var candidate = new SeatReservation { Id = 10, SeatId = 100, Date = date, FromId = 1, ToId = 2 };

            await service.ValidateReservation(candidate);
        }

        [Fact]
        public async Task ValidateReservation_NullFromOrTo_ShouldPass()
        {
            var db = CreateInMemoryDb(nameof(ValidateReservation_NullFromOrTo_ShouldPass));
            var service = new SeatReservationsService(new NullLogger<SeatReservationsService>(), db);
            var candidate = new SeatReservation { Id = 10, SeatId = 100, Date = DateTime.UtcNow.Date, FromId = null, ToId = 2 };
            await service.ValidateReservation(candidate);
        }
    }
}


