using Api.AspNetCore.Exceptions;
using Data.Repository.Helpers;
using Microsoft.EntityFrameworkCore;
using Ticketing.Data.TicketDb.DatabaseContext;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Models.Dtos;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ticketing.Services
{
	public class TrainSchedulesService
	{
		private readonly TicketDbContext db;

		public TrainSchedulesService(ILogger<TrainSchedulesService> logger,
			TicketDbContext db)
		{
			this.db = db;
		}

		public async Task<TrainScheduleActivationResponse> Activate(long scheduleId)
		{
			var schedule = await db.Set<TrainSchedule>()
				.Include(_ => _.Train)
				.FirstOrDefaultAsync(_ => _.Id == scheduleId);

			if (schedule == null)
				throw new BadRequestException("TrainSchedule not found");

			var routeId = schedule.Train?.RouteId;
			if (!routeId.HasValue)
				throw new BadRequestException("Train has no route");

		var routeStations = await db.Set<RouteStation>()
			.Where(_ => _.RouteId == routeId)
			.OrderBy(_ => _.Order)
			.ToListAsync();

		if (routeStations.Count < 2)
			throw new BadRequestException("Route must contain at least 2 stations");

		var stationPairs = new List<(long fromId, long toId, DateTime departureTime)>();
		for (int i = 0; i < routeStations.Count - 1; i++)
		{
			var fromStation = routeStations[i];
			var departureTime = fromStation.Departure.HasValue 
				? schedule.Date.Date + (fromStation.Departure.Value - new DateTime(1900, 1, 1))
				: schedule.Date.Date;
			stationPairs.Add((fromStation.Id, routeStations[i + 1].Id, departureTime));
		}

			var trainWagons = await db.Set<TrainWagon>()
				.Include(_ => _.Wagon)
				.Where(_ => _.TrainScheduleId == schedule.Id)
				.ToListAsync();

			int seatsCreated = 0;
			int segmentsCreated = 0;

			foreach (var trainWagon in trainWagons)
			{
				var seatCount = trainWagon.Wagon?.SeatCount ?? 0;
				if (seatCount <= 0)
					continue;

				var wagonId = trainWagon.Id;
				var existingSeats = await db.Set<Seat>().Where(_ => _.WagonId == wagonId).ToListAsync();
				var existingNumbers = new HashSet<string>(existingSeats.Where(_ => _.Number != null).Select(_ => _.Number!));

				var newSeats = new List<Seat>();
				for (int n = 1; n <= seatCount; n++)
				{
					var num = n.ToString();
					if (!existingNumbers.Contains(num))
					{
						newSeats.Add(new Seat
						{
							Number = num,
							Class = 0,
							WagonId = wagonId,
							TypeId = null
						});
					}
				}

				if (newSeats.Count > 0)
				{
					await db.Set<Seat>().AddRangeAsync(newSeats);
					seatsCreated += newSeats.Count;
					await db.SaveChangesAsync();
					existingSeats.AddRange(newSeats);
				}

				var existingSegments = await db.Set<SeatSegment>()
					.Where(_ => _.TrainScheduleId == schedule.Id && _.WagonId == wagonId)
					.Select(_ => new { _.SeatId, _.FromId, _.ToId })
					.ToListAsync();

				var existingSegmentKeys = new HashSet<string>(existingSegments
					.Where(_ => _.SeatId.HasValue && _.FromId.HasValue && _.ToId.HasValue)
					.Select(_ => $"{_.SeatId.GetValueOrDefault()}:{_.FromId.GetValueOrDefault()}:{_.ToId.GetValueOrDefault()}"));

				var toAdd = new List<SeatSegment>();
				foreach (var seat in existingSeats)
				{
					foreach (var pair in stationPairs)
					{
						var keyStr = $"{seat.Id}:{pair.fromId}:{pair.toId}";
						if (!existingSegmentKeys.Contains(keyStr))
						{
							toAdd.Add(new SeatSegment
							{
								SeatId = seat.Id,
								FromId = pair.fromId,
								ToId = pair.toId,
								TrainId = schedule.TrainId,
								WagonId = wagonId,
								TrainScheduleId = schedule.Id,
								Price = 0,
								Departure = pair.departureTime
							});
						}
					}
				}

				if (toAdd.Count > 0)
				{
					await db.Set<SeatSegment>().AddRangeAsync(toAdd);
					segmentsCreated += toAdd.Count;
					await db.SaveChangesAsync();
				}
			}

			schedule.Active = true;
			await db.SaveChangesAsync();

			return new TrainScheduleActivationResponse
			{
				ScheduleId = schedule.Id,
				Wagons = trainWagons.Count,
				SeatsCreated = seatsCreated,
				SegmentsCreated = segmentsCreated,
				StationPairs = stationPairs.Count
			};
		}

		public async Task<TrainScheduleDatesResponseDto> CreateSchedulesByDatesAsync(TrainScheduleDatesRequestDto request)
		{
			// Get train with plan and wagons
			var train = await db.Set<Train>()
				.Include(_ => _.Plan)
				.ThenInclude(_ => _.Wagons)
				.ThenInclude(_ => _.Wagon)
				.Include(_ => _.Route)
				.FirstOrDefaultAsync(_ => _.Id == request.TrainId);

			if (train == null)
				throw new BadRequestException("Train not found");

			if (train.Plan?.Wagons == null || !train.Plan.Wagons.Any())
				throw new BadRequestException("Train has no plan wagons");

			if (!train.RouteId.HasValue)
				throw new BadRequestException("Train has no route");

			// Get route stations for seat segments
			var routeStations = await db.Set<RouteStation>()
				.Where(_ => _.RouteId == train.RouteId)
				.OrderBy(_ => _.Order)
				.ToListAsync();

			if (routeStations.Count < 2)
				throw new BadRequestException("Route must contain at least 2 stations");

			var response = new TrainScheduleDatesResponseDto();

			foreach (var date in request.Dates)
			{
				// Check if schedule already exists for this date
				var existingSchedule = await db.Set<TrainSchedule>()
					.FirstOrDefaultAsync(_ => _.TrainId == request.TrainId && _.Date.Date == date.Date.ToUtc());

				if (existingSchedule == null)
				{

					// Create TrainSchedule
					var schedule = new TrainSchedule
					{
						Date = date.Date,
						Active = false,
						TrainId = request.TrainId
					};

					db.Set<TrainSchedule>().Add(schedule);
					await db.SaveChangesAsync();
					existingSchedule = schedule;
                    response.SchedulesCreated++;
                }
				response.ScheduleIds.Add(existingSchedule.Id);				

				// Create TrainWagons based on plan wagons
				foreach (var planWagon in train.Plan.Wagons)
				{
					// Check if train wagon already exists for this schedule
					var existingTrainWagon = await db.Set<TrainWagon>()
						.FirstOrDefaultAsync(_ => _.TrainScheduleId == existingSchedule.Id && _.Number == planWagon.Number);

					TrainWagon trainWagon;
					if (existingTrainWagon == null)
					{
						trainWagon = new TrainWagon
						{
							Number = planWagon.Number,
							TrainScheduleId = existingSchedule.Id,
							WagonId = planWagon.WagonId
						};

						db.Set<TrainWagon>().Add(trainWagon);
						await db.SaveChangesAsync();
						response.TrainWagonsCreated++;
					}
					else
					{
						trainWagon = existingTrainWagon;
					}

					// Create Seats based on wagon seat count
					var wagon = planWagon.Wagon;
					if (wagon?.SeatCount > 0)
					{
						// Get existing seats for this train wagon
						var existingSeats = await db.Set<Seat>().Where(_ => _.WagonId == trainWagon.Id).ToListAsync();
						var existingNumbers = new HashSet<string>(existingSeats.Where(_ => _.Number != null).Select(_ => _.Number!));

						var newSeats = new List<Seat>();
						for (int n = 1; n <= wagon.SeatCount; n++)
						{
							var seatNumber = n.ToString();
							if (!existingNumbers.Contains(seatNumber))
							{
								newSeats.Add(new Seat
								{
									Number = seatNumber,
									Class = 0,
									WagonId = trainWagon.Id,
									TypeId = null
								});
							}
						}

						if (newSeats.Count > 0)
						{
							await db.Set<Seat>().AddRangeAsync(newSeats);
							await db.SaveChangesAsync();
							response.SeatsCreated += newSeats.Count;
							existingSeats.AddRange(newSeats);
						}

						if (planWagon.Number == "10")
						{

						}

						// Check existing seat segments for this train wagon
						var existingSegments = await db.Set<SeatSegment>()
							.Where(_ => _.TrainScheduleId == existingSchedule.Id && _.WagonId == trainWagon.Id)
							.Select(_ => new { _.SeatId, _.FromId, _.ToId })
							.ToListAsync();

						var existingSegmentKeys = new HashSet<string>(existingSegments
							.Where(_ => _.SeatId.HasValue && _.FromId.HasValue && _.ToId.HasValue)
							.Select(_ => $"{_.SeatId.GetValueOrDefault()}:{_.FromId.GetValueOrDefault()}:{_.ToId.GetValueOrDefault()}"));

						// Create station pairs with departure times for this specific schedule date
						var stationPairs = new List<(long? fromId, long? toId, DateTime departureTime)>();
						for (int i = 0; i < routeStations.Count - 1; i++)
						{
							var fromStation = routeStations[i];
							var departureTime = fromStation.Departure.HasValue 
								? date.Date + (fromStation.Departure.Value - new DateTime(1900, 1, 1))
								: date.Date;
							stationPairs.Add((fromStation.Id, routeStations[i + 1].Id, departureTime));
						}

						// Create SeatSegments for each seat and station pair
						var toAdd = new List<SeatSegment>();
						foreach (var seat in existingSeats)
						{
							foreach (var pair in stationPairs)
							{
								var keyStr = $"{seat.Id}:{pair.fromId}:{pair.toId}";
								if (!existingSegmentKeys.Contains(keyStr))
								{
									toAdd.Add(new SeatSegment
									{
										SeatId = seat.Id,
										FromId = pair.fromId,
										ToId = pair.toId,
										TrainId = existingSchedule.TrainId,
										WagonId = trainWagon.Id,
										TrainScheduleId = existingSchedule.Id,
										Price = 0,
										Departure = pair.departureTime
									});
								}
							}
						}

						if (toAdd.Count > 0)
						{
							await db.Set<SeatSegment>().AddRangeAsync(toAdd);
							await db.SaveChangesAsync();
							response.SeatSegmentsCreated += toAdd.Count;
						}
					}
				}
			}

			return response;
		}
	}
}

