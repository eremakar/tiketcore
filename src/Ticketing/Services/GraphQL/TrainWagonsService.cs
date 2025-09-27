using HotChocolate;
using HotChocolate.Authorization;
using Ticketing.Models.Dtos;
using Microsoft.EntityFrameworkCore;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Services.GraphQL
{
    public class TrainWagonsService : RestService2<TrainWagon, long, TrainWagonDto, TrainWagonQuery, TrainWagonMap>
    {
        private readonly TicketDbContext db;

        public TrainWagonsService(ILogger<RestServiceBase<TrainWagon, long>> logger,
            IDapperDbContext restDapperDb,
            TicketDbContext restDb,
            TrainWagonMap map)
            : base(logger,
                restDapperDb,
                restDb,
                "TrainWagons",
                map)
        {
            this.db = restDb;
        }

        public override async Task<PagedList<TrainWagonDto>> SearchAsync(TrainWagonQuery query)
        {
            return await SearchUsingEfAsync(query, _ => _.
                Include(_ => _.TrainSchedule).
                Include(_ => _.Wagon));
        }

        /// <summary>
        /// Auto-generates seats for a train wagon based on the wagon's seatCount.
        /// </summary>
        /// <param name="trainWagonId">The ID of the train wagon.</param>
        /// <returns>The number of seats generated.</returns>
        public async Task<int> GenerateSeatsAsync(long trainWagonId)
        {
            // Get train wagon with wagon details
            var trainWagon = await db.Set<TrainWagon>()
                .Include(tw => tw.Wagon)
                .FirstOrDefaultAsync(tw => tw.Id == trainWagonId);

            if (trainWagon == null)
            {
                throw new ArgumentException($"Train wagon with id {trainWagonId} not found");
            }

            if (trainWagon.Wagon == null)
            {
                throw new ArgumentException("Train wagon has no associated wagon");
            }

            var seatCount = trainWagon.Wagon.SeatCount;
            if (seatCount <= 0)
            {
                throw new ArgumentException("Wagon seat count must be greater than 0");
            }

            // Get existing seats for this train wagon
            var existingSeats = await db.Set<Seat>()
                .Where(s => s.WagonId == trainWagonId)
                .ToListAsync();

            var existingNumbers = new HashSet<string>(existingSeats.Where(s => s.Number != null).Select(s => s.Number!));

            // Generate new seats
            var newSeats = new List<Seat>();
            for (int n = 1; n <= seatCount; n++)
            {
                var seatNumber = n.ToString();
                if (!existingNumbers.Contains(seatNumber))
                {
                    newSeats.Add(new Seat
                    {
                        Number = seatNumber,
                        Class = 0,
                        WagonId = trainWagonId,
                        TypeId = null
                    });
                }
            }

            if (newSeats.Count > 0)
            {
                await db.Set<Seat>().AddRangeAsync(newSeats);
                await db.SaveChangesAsync();
            }

            return newSeats.Count;
        }
    }
}
