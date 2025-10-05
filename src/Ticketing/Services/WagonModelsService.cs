using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Services
{
    public class WagonModelsService
    {
        private readonly TicketDbContext db;
        private readonly ILogger<WagonModelsService> logger;

        public WagonModelsService(TicketDbContext db, ILogger<WagonModelsService> logger)
        {
            this.db = db;
            this.logger = logger;
        }

        /// <summary>
        /// Auto-generates seats for a wagon model based on the wagon model's seatCount.
        /// </summary>
        /// <param name="wagonModelId">The ID of the wagon model.</param>
        /// <returns>The number of seats generated.</returns>
        public async Task<int> GenerateSeatsAsync(long wagonModelId)
        {
            // Get wagon model
            var wagonModel = await db.Set<WagonModel>()
                .FirstOrDefaultAsync(wm => wm.Id == wagonModelId);

            if (wagonModel == null)
            {
                throw new ArgumentException($"Wagon model with id {wagonModelId} not found");
            }

            var seatCount = wagonModel.SeatCount;
            if (seatCount <= 0)
            {
                throw new ArgumentException("Wagon model seat count must be greater than 0");
            }

            // Load seat types
            var lowerSeatType = await db.Set<SeatType>()
                .FirstOrDefaultAsync(st => st.Name == "Нижний");
            var upperSeatType = await db.Set<SeatType>()
                .FirstOrDefaultAsync(st => st.Name == "Верхний");

            if (lowerSeatType == null || upperSeatType == null)
            {
                throw new InvalidOperationException("Seat types 'Нижний' and 'Верхний' must exist in the database");
            }

            // Get existing seats for this wagon model
            var existingSeats = await db.Set<Seat>()
                .Where(s => s.WagonId == wagonModelId)
                .ToListAsync();

            var existingSeatsByNumber = existingSeats
                .Where(s => s.Number != null)
                .ToDictionary(s => s.Number!, s => s);

            // Generate new seats or update existing ones with alternating types
            var newSeats = new List<Seat>();
            var updatedCount = 0;
            
            for (int n = 1; n <= seatCount; n++)
            {
                var seatNumber = n.ToString();
                // Alternate between lower (odd) and upper (even)
                var typeId = (n % 2 == 1) ? lowerSeatType.Id : upperSeatType.Id;

                if (existingSeatsByNumber.TryGetValue(seatNumber, out var existingSeat))
                {
                    // Update existing seat
                    existingSeat.Class = 0;
                    existingSeat.TypeId = typeId;
                    updatedCount++;
                }
                else
                {
                    // Create new seat
                    newSeats.Add(new Seat
                    {
                        Number = seatNumber,
                        Class = 0,
                        WagonId = wagonModelId,
                        TypeId = typeId
                    });
                }
            }

            if (newSeats.Count > 0)
            {
                await db.Set<Seat>().AddRangeAsync(newSeats);
            }

            if (newSeats.Count > 0 || updatedCount > 0)
            {
                await db.SaveChangesAsync();
            }

            return newSeats.Count + updatedCount;
        }
    }
}
