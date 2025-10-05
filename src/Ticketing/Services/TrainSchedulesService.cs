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
		private readonly WorkflowTaskService workflowTaskService;

		public TrainSchedulesService(ILogger<TrainSchedulesService> logger,
			TicketDbContext db,
			WorkflowTaskService workflowTaskService)
		{
			this.db = db;
			this.workflowTaskService = workflowTaskService;
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
				var scheduleDate = schedule.Date.ToUtc();
				var departureTime = fromStation.Departure.HasValue
					? scheduleDate + (fromStation.Departure.Value - new DateTime(1900, 1, 1))
					: scheduleDate;
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

				var wagonModelId = trainWagon.Wagon!.Id; // WagonModel.Id for Seat
				var trainWagonId = trainWagon.Id; // TrainWagon.Id for SeatSegment

				var existingSeats = await db.Set<Seat>().Where(_ => _.WagonId == wagonModelId).ToListAsync();
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
							WagonId = wagonModelId,
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
					.Where(_ => _.TrainScheduleId == schedule.Id && _.WagonId == trainWagonId)
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
								WagonId = trainWagonId,
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
			// Start task if provided
			await workflowTaskService.StartTaskAsync(request.WorkflowTaskId);

			await workflowTaskService.LogAsync(
				request.WorkflowTaskId,
				$"Starting schedule creation for train {request.TrainId}, {request.Dates.Count} dates",
				LogSeverity.Info);

			try
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

				await workflowTaskService.LogAsync(
					request.WorkflowTaskId,
					$"Train validated: {train.Plan.Wagons.Count} wagons in plan",
					LogSeverity.Info);

				// Get route stations for seat segments
				var routeStations = await db.Set<RouteStation>()
					.Where(_ => _.RouteId == train.RouteId)
					.OrderBy(_ => _.Order)
					.ToListAsync();

				if (routeStations.Count < 2)
					throw new BadRequestException("Route must contain at least 2 stations");

				// Pre-load all existing Seats for all wagons in plan
				var wagonIds = train.Plan.Wagons.Select(_ => _.WagonId).ToList();
				var allSeats = await db.Set<Seat>()
					.Where(_ => wagonIds.Contains(_.WagonId))
					.ToListAsync();

				var seatsByWagonId = allSeats.GroupBy(_ => _.WagonId).ToDictionary(_ => _.Key, _ => _.ToList());

				await workflowTaskService.LogAsync(
					request.WorkflowTaskId,
					$"Loaded seats for {wagonIds.Count} wagons, total {allSeats.Count} seats",
					LogSeverity.Info);

				// Create station pairs template (will apply date later)
				var stationPairsTemplate = new List<(long? fromId, long? toId, TimeSpan? departureOffset)>();
				for (int i = 0; i < routeStations.Count - 1; i++)
				{
					var fromStation = routeStations[i];
					var departureOffset = fromStation.Departure.HasValue
						? fromStation.Departure.Value - new DateTime(1900, 1, 1)
						: TimeSpan.Zero;
					stationPairsTemplate.Add((fromStation.Id, routeStations[i + 1].Id, departureOffset));
				}

				var response = new TrainScheduleDatesResponseDto();
				var totalDates = request.Dates.Count;
				var processedDates = 0;

				foreach (var date in request.Dates)
				{
					// Check if schedule already exists for this date
					var startDate = date.ToUtc();
					var endDate = startDate.AddDays(1);
					var existingSchedule = await db.Set<TrainSchedule>()
						.FirstOrDefaultAsync(_ => _.TrainId == request.TrainId && startDate <= _.Date && _.Date < endDate);

					if (existingSchedule != null)
					{
						// Schedule exists, skip creation
						response.ScheduleIds.Add(existingSchedule.Id);

						processedDates++;
						var percent = (int)((processedDates / (double)totalDates) * 100);
						await workflowTaskService.UpdateProgressAsync(
							request.WorkflowTaskId,
							percent,
							$"Schedule for {startDate:yyyy-MM-dd} already exists (ID: {existingSchedule.Id})");
						await workflowTaskService.LogAsync(
							request.WorkflowTaskId,
							$"Skipped {startDate:yyyy-MM-dd} - schedule already exists",
							LogSeverity.Info);

						continue;
					}

					// Validate seats exist for all wagons before creating schedule
					foreach (var planWagon in train.Plan.Wagons)
					{
						if (!seatsByWagonId.TryGetValue(planWagon.WagonId, out var seats) || seats.Count == 0)
							throw new BadRequestException($"No seats found for wagon {planWagon.WagonId}. Cannot create schedule.");
					}

					// Use execution strategy for transaction
					var strategy = db.Database.CreateExecutionStrategy();
					await strategy.ExecuteAsync(async () =>
					{
						await using var transaction = await db.Database.BeginTransactionAsync();
						try
						{
							var trainWagonsToAdd = new List<TrainWagon>();
							var seatSegmentsToAdd = new List<SeatSegment>();

							// Create TrainSchedule
							var newSchedule = new TrainSchedule
							{
								Date = startDate,
								Active = false,
								TrainId = request.TrainId
							};
							db.Set<TrainSchedule>().Add(newSchedule);
							await db.SaveChangesAsync(); // Get schedule ID

							// Create TrainWagons
							foreach (var planWagon in train.Plan.Wagons)
							{
								var trainWagon = new TrainWagon
								{
									Number = planWagon.Number,
									TrainScheduleId = newSchedule.Id,
									WagonId = planWagon.WagonId
								};
								trainWagonsToAdd.Add(trainWagon);
							}

							await db.Set<TrainWagon>().AddRangeAsync(trainWagonsToAdd);
							await db.SaveChangesAsync(); // Get wagon IDs

							// Create SeatSegments for each wagon
							for (int i = 0; i < train.Plan.Wagons.Count; i++)
							{
								var planWagon = train.Plan.Wagons[i];
								var trainWagon = trainWagonsToAdd[i];
								var seats = seatsByWagonId[planWagon.WagonId];

								foreach (var seat in seats)
								{
									foreach (var pair in stationPairsTemplate)
									{
										var departureTime = startDate + (pair.departureOffset ?? TimeSpan.Zero);
										seatSegmentsToAdd.Add(new SeatSegment
										{
											SeatId = seat.Id,
											FromId = pair.fromId,
											ToId = pair.toId,
											TrainId = newSchedule.TrainId,
											WagonId = trainWagon.Id,
											TrainScheduleId = newSchedule.Id,
											Price = 0,
											Departure = departureTime
										});
									}
								}
							}

							if (seatSegmentsToAdd.Count > 0)
							{
								await db.Set<SeatSegment>().AddRangeAsync(seatSegmentsToAdd);
								await db.SaveChangesAsync();
							}

							// Commit transaction - all successful
							await transaction.CommitAsync();

							response.SchedulesCreated++;
							response.TrainWagonsCreated += trainWagonsToAdd.Count;
							response.SeatSegmentsCreated += seatSegmentsToAdd.Count;
							response.ScheduleIds.Add(newSchedule.Id);

							processedDates++;
							var percent = (int)((processedDates / (double)totalDates) * 100);
							await workflowTaskService.UpdateProgressAsync(
								request.WorkflowTaskId,
								percent,
								$"Created schedule for {startDate:yyyy-MM-dd} (ID: {newSchedule.Id})");
							await workflowTaskService.LogAsync(
								request.WorkflowTaskId,
								$"Schedule {newSchedule.Id} created for {startDate:yyyy-MM-dd}: {trainWagonsToAdd.Count} wagons, {seatSegmentsToAdd.Count} segments",
								LogSeverity.Info,
								data: new
								{
									ScheduleId = newSchedule.Id,
									Date = startDate,
									Wagons = trainWagonsToAdd.Count,
									Segments = seatSegmentsToAdd.Count
								});
						}
						catch (Exception ex)
						{
							await transaction.RollbackAsync();

							await workflowTaskService.LogAsync(
								request.WorkflowTaskId,
								$"Failed to create schedule for {startDate:yyyy-MM-dd}: {ex.Message}",
								LogSeverity.Error,
								data: new { Date = startDate, Error = ex.Message });

							throw; // Re-throw to fail fast
						}
					});
				}

				await workflowTaskService.LogAsync(
					request.WorkflowTaskId,
					$"Completed: {response.SchedulesCreated} schedules created, {response.TrainWagonsCreated} wagons, {response.SeatSegmentsCreated} segments",
					LogSeverity.Info,
					data: new
					{
						TotalDates = totalDates,
						SchedulesCreated = response.SchedulesCreated,
						TrainWagonsCreated = response.TrainWagonsCreated,
						SeatSegmentsCreated = response.SeatSegmentsCreated,
						ScheduleIds = response.ScheduleIds
					});

				await workflowTaskService.CompleteTaskAsync(
					request.WorkflowTaskId,
					output: response,
					message: $"{response.SchedulesCreated} schedules created successfully");

				return response;
			}
			catch (Exception ex)
			{
				await workflowTaskService.FailTaskAsync(
					request.WorkflowTaskId,
					$"Schedule creation failed: {ex.Message}",
					ex);
				throw;
			}
		}
	}
}

