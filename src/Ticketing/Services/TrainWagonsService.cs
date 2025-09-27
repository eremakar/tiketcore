using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Services
{
    public class TrainWagonsService
    {
        private readonly TicketDbContext db;
        private readonly ILogger<TrainWagonsService> logger;

        public TrainWagonsService(TicketDbContext db, ILogger<TrainWagonsService> logger)
        {
            this.db = db;
            this.logger = logger;
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
