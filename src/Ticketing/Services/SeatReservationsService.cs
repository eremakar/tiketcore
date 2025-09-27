using Ticketing.Models.Dtos;
using Microsoft.EntityFrameworkCore;
using Api.AspNetCore.Exceptions;
using Ticketing.Data.TicketDb.DatabaseContext;
using Data.Repository.Dapper;
using Ticketing.Mappings;
using Ticketing.Data.TicketDb.Entities;

namespace Ticketing.Services
{
    /// <summary>
    /// Validation service for seat and seat-count reservations.
    /// </summary>
    public class SeatReservationsService
    {
        private readonly TicketDbContext db;

        /// <summary>
        /// Creates a new instance of the validation service.
        /// </summary>
        public SeatReservationsService(ILogger<SeatReservationsService> logger,
            TicketDbContext restDb)
        {
            this.db = restDb;
        }
        /// <summary>
        /// Validates a single-seat reservation against overlapping segments and wagon capacity.
        /// </summary>
        public async Task ValidateReservation(SeatReservation source)
        {
            if (source.SeatId == null || source.FromId == null || source.ToId == null || source.TrainScheduleId == null)
                return;            

            // Capacity validation against wagon capacity using SeatCountSegments and SeatSegments within same wagon
            if (source.WagonId.HasValue)
            {
                await ValidateCapacityForInterval(
                    trainScheduleId: source.TrainScheduleId.Value,
                    trainWagonId: source.WagonId.Value,
                    fromRouteStationId: source.FromId.Value,
                    toRouteStationId: source.ToId.Value,
                    additionalSeatsRequested: 1,
                    excludeSeatReservationId: source.Id,
                    excludeSeatCountReservationId: null);
            }

            // Check overlaps using SeatReservation and only occupied SeatSegments (ignore pre-populated adjacent segments)
            var existingSeatReservations = await db.Set<SeatReservation>()
                .AsNoTracking()
                .Where(r => r.SeatId == source.SeatId
                            && r.TrainScheduleId == source.TrainScheduleId
                            && r.Id != source.Id)
                .Select(r => new { r.FromId, r.ToId })
                .ToListAsync();

            var existingReservedSeatSegments = await db.Set<SeatSegment>()
                .AsNoTracking()
                .Where(s => s.SeatId == source.SeatId
                            && s.TrainScheduleId == source.TrainScheduleId
                            && s.SeatReservationId != source.Id
                            && (s.SeatReservationId != null || s.TicketId != null))
                .Select(s => new { s.FromId, s.ToId })
                .ToListAsync();

            var intervalSources = existingSeatReservations
                .Concat(existingReservedSeatSegments)
                .ToList();

            // Do not return here; even if no seat-level overlaps, capacity must be validated

            var routeStationIds = new HashSet<long> { source.FromId.Value, source.ToId.Value };
            foreach (var r in intervalSources)
            {
                if (r.FromId.HasValue && !routeStationIds.Contains(r.FromId.Value)) routeStationIds.Add(r.FromId.Value);
                if (r.ToId.HasValue && !routeStationIds.Contains(r.ToId.Value)) routeStationIds.Add(r.ToId.Value);
            }

            var orders = await db.Set<RouteStation>()
                .AsNoTracking()
                .Where(rs => routeStationIds.Contains(rs.Id))
                .Select(rs => new { rs.Id, rs.Order })
                .ToDictionaryAsync(x => x.Id, x => x.Order);

            var newFromOrder = orders[source.FromId.Value];
            var newToOrder = orders[source.ToId.Value];
            var newStart = Math.Min(newFromOrder, newToOrder);
            var newEnd = Math.Max(newFromOrder, newToOrder);

            foreach (var r in intervalSources)
            {
                if (!r.FromId.HasValue || !r.ToId.HasValue)
                    continue;

                if (!orders.TryGetValue(r.FromId.Value, out var reservationFromOrder))
                    continue;
                if (!orders.TryGetValue(r.ToId.Value, out var reservationToOrder))
                    continue;

                var reservationStart = Math.Min(reservationFromOrder, reservationToOrder);
                var reservationEnd = Math.Max(reservationFromOrder, reservationToOrder);

                if (newStart < reservationEnd && reservationStart < newEnd)
                {
                    throw new BadRequestException("Место уже забронировано на выбранный участок маршрута");
                }
            }
        }

        /// <summary>
        /// Validates a seat-count reservation against wagon capacity and overlapping segments.
        /// </summary>
        public async Task ValidateReservation(SeatCountReservation source)
        {
            if (source.FromId == null || source.ToId == null || source.TrainScheduleId == null || source.WagonId == null)
                return;

            if (source.SeatCount <= 0)
                throw new BadRequestException("Некорректное количество мест для бронирования");

            await ValidateCapacityForInterval(
                trainScheduleId: source.TrainScheduleId.Value,
                trainWagonId: source.WagonId.Value,
                fromRouteStationId: source.FromId.Value,
                toRouteStationId: source.ToId.Value,
                additionalSeatsRequested: source.SeatCount,
                excludeSeatReservationId: null,
                excludeSeatCountReservationId: source.Id);
        }

        private async Task ValidateCapacityForInterval(long trainScheduleId,
            long trainWagonId,
            long fromRouteStationId,
            long toRouteStationId,
            int additionalSeatsRequested,
            long? excludeSeatReservationId,
            long? excludeSeatCountReservationId)
        {
            // Resolve capacity from TrainWagon -> Wagon.SeatCount
            var trainWagon = await db.Set<TrainWagon>()
                .AsNoTracking()
                .Include(tw => tw.Wagon)
                .FirstOrDefaultAsync(tw => tw.Id == trainWagonId && tw.TrainScheduleId == trainScheduleId);

            if (trainWagon == null || trainWagon.Wagon == null)
                throw new BadRequestException("Неверно указан вагон или расписание");

            var capacity = trainWagon.Wagon.SeatCount;
            if (additionalSeatsRequested > capacity)
                throw new BadRequestException("Запрошено мест больше, чем вместимость вагона");

            // Build order map for overlap detection
            var routeStationIds = new HashSet<long> { fromRouteStationId, toRouteStationId };

            var countSegments = await db.Set<SeatCountSegment>()
                .AsNoTracking()
                .Where(s => s.TrainScheduleId == trainScheduleId
                            && s.WagonId == trainWagonId)
                .Select(s => new { s.FromId, s.ToId, s.SeatCount, s.Id })
                .ToListAsync();

            foreach (var r in countSegments)
            {
                if (r.FromId.HasValue) routeStationIds.Add(r.FromId.Value);
                if (r.ToId.HasValue) routeStationIds.Add(r.ToId.Value);
            }

            var orders = await db.Set<RouteStation>()
                .AsNoTracking()
                .Where(rs => routeStationIds.Contains(rs.Id))
                .Select(rs => new { rs.Id, rs.Order })
                .ToDictionaryAsync(x => x.Id, x => x.Order);

            var newFromOrder = orders[fromRouteStationId];
            var newToOrder = orders[toRouteStationId];
            var newStart = Math.Min(newFromOrder, newToOrder);
            var newEnd = Math.Max(newFromOrder, newToOrder);

            // Sum SeatCount over overlapping SeatCountSegments only
            var usedFromCountSegments = 0;
            foreach (var r in countSegments)
            {
                if (excludeSeatCountReservationId.HasValue)
                {
                    // We cannot filter by join table easily here; rely on controller to avoid double-count during self-update
                }
                if (!r.FromId.HasValue || !r.ToId.HasValue) continue;
                if (!orders.TryGetValue(r.FromId.Value, out var rf)) continue;
                if (!orders.TryGetValue(r.ToId.Value, out var rt)) continue;
                var rs = Math.Min(rf, rt);
                var re = Math.Max(rf, rt);
                if (newStart < re && rs < newEnd)
                    usedFromCountSegments += r.SeatCount;
            }

            // Per requested rule: validate as wagon.SeatCount + additionalSeatsRequested < sum(SeatCountSegments.SeatCount)
            if (capacity + additionalSeatsRequested < usedFromCountSegments)
                throw new BadRequestException("Недостаточно мест в вагоне на выбранный участок маршрута");
        }

		/// <summary>
		/// Creates a seat reservation and marks all adjacent seat segments between the given route stations.
		/// Example: for A-B-C-D-E and reservation A-C, segments A-B and B-C will be created.
		/// </summary>
		public async Task<SeatReservationResponse> Reserve(long trainScheduleId,
			long trainWagonId,
			long seatId,
			long fromRouteStationId,
			long toRouteStationId,
			double price = 0)
		{
			// Load all single-hop segments for this seat/wagon/schedule
			var allSeatSegments = await db.Set<SeatSegment>()
				.AsNoTracking()
				.Where(s => s.TrainScheduleId == trainScheduleId
					&& s.WagonId == trainWagonId
					&& s.SeatId == seatId)
				.Select(s => new { s.Id, s.FromId, s.ToId, s.TrainId, s.SeatReservationId, s.TicketId })
				.ToListAsync();

			if (allSeatSegments.Count == 0)
				return new SeatReservationResponse { Result = SeatReservationResult.NotFound, Message = "Нет сегментов для указанного места/вагона/расписания" };

			// Build order map using RouteStation to determine interval inclusion
			var routeStationIds = new HashSet<long> { fromRouteStationId, toRouteStationId };
			foreach (var s in allSeatSegments)
			{
				if (s.FromId.HasValue) routeStationIds.Add(s.FromId.Value);
				if (s.ToId.HasValue) routeStationIds.Add(s.ToId.Value);
			}
			var orders = await db.Set<RouteStation>()
				.AsNoTracking()
				.Where(rs => routeStationIds.Contains(rs.Id))
				.Select(rs => new { rs.Id, rs.Order })
				.ToDictionaryAsync(x => x.Id, x => x.Order);

			if (!orders.ContainsKey(fromRouteStationId) || !orders.ContainsKey(toRouteStationId))
				return new SeatReservationResponse { Result = SeatReservationResult.InvalidInput, Message = "Не удалось определить порядок станций для интервала" };

			var newFromOrder = orders[fromRouteStationId];
			var newToOrder = orders[toRouteStationId];
			var forward = newFromOrder < newToOrder;
			var newStart = Math.Min(newFromOrder, newToOrder);
			var newEnd = Math.Max(newFromOrder, newToOrder);

			var pathSegmentIds = new List<long>();
			var neededHops = newEnd - newStart;
			// Build a lookup for quick find of segment by hop order index
			var hopToSegment = new Dictionary<int, long>();
			foreach (var s in allSeatSegments)
			{
				if (!s.FromId.HasValue || !s.ToId.HasValue) continue;
				if (!orders.TryGetValue(s.FromId.Value, out var rf)) continue;
				if (!orders.TryGetValue(s.ToId.Value, out var rt)) continue;
				if (Math.Abs(rf - rt) != 1) continue;
				var segForward = rf < rt;
				if (segForward != forward) continue;
				var rs = Math.Min(rf, rt);
				var re = Math.Max(rf, rt);
				if (rs >= newStart && re <= newEnd)
				{
					// rs corresponds to hop index within [newStart, newEnd)
					var hopIndex = rs - newStart;
					if (!hopToSegment.ContainsKey(hopIndex))
						hopToSegment[hopIndex] = s.Id;
				}
			}

			for (var i = 0; i < neededHops; i++)
			{
				if (!hopToSegment.TryGetValue(i, out var segId))
					return new SeatReservationResponse { Result = SeatReservationResult.IncompleteSegments, Message = "Не все сегменты найдены для указанного интервала" };
				pathSegmentIds.Add(segId);
			}

			// Occupancy check before creating reservation
			var occupied = allSeatSegments
				.Where(s => pathSegmentIds.Contains(s.Id))
				.Any(s => s.SeatReservationId.HasValue || s.TicketId.HasValue);
			if (occupied)
				return new SeatReservationResponse { Result = SeatReservationResult.SegmentsOccupied, Message = "Один или более сегментов уже заняты" };

			var anyTrainId = allSeatSegments.Select(x => x.TrainId).FirstOrDefault();

			var reservation = new SeatReservation
			{
				Date = DateTime.UtcNow,
				Price = price,
				FromId = fromRouteStationId,
				ToId = toRouteStationId,
				TrainId = anyTrainId,
				WagonId = trainWagonId,
				SeatId = seatId,
				TrainScheduleId = trainScheduleId
			};

			// Validate reservation before marking segments
			try
			{
				await ValidateReservation(reservation);
			}
			catch (BadRequestException ex)
			{
				return new SeatReservationResponse { Result = SeatReservationResult.ValidationFailed, Message = ex.Message };
			}

			await db.Set<SeatReservation>().AddAsync(reservation);
			await db.SaveChangesAsync();

			if (pathSegmentIds.Count > 0)
			{
				var segmentsToMark = await db.Set<SeatSegment>()
					.Where(s => pathSegmentIds.Contains(s.Id))
					.ToListAsync();
				foreach (var seg in segmentsToMark)
				{
					seg.SeatReservationId = reservation.Id;
				}
				await db.SaveChangesAsync();
				reservation.Segments = segmentsToMark;
			}

			return new SeatReservationResponse { Result = SeatReservationResult.Success, ReservationId = reservation.Id };
		}
    }
}
