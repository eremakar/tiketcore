namespace Ticketing.Models.Dtos
{
	public enum SeatReservationResult
	{
		Success = 0,
		NotFound = 1,
		InvalidInput = 2,
		IncompleteSegments = 3,
		SegmentsOccupied = 4,
		ValidationFailed = 5
	}

	public class SeatReservationResponse
	{
		public SeatReservationResult Result { get; set; }
		public long? ReservationId { get; set; }
		public string? Message { get; set; }
	}
}

