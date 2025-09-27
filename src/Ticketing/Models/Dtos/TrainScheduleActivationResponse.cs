namespace Ticketing.Models.Dtos
{
	public class TrainScheduleActivationResponse
	{
		public long ScheduleId { get; set; }
		public int Wagons { get; set; }
		public int SeatsCreated { get; set; }
		public int SegmentsCreated { get; set; }
		public int StationPairs { get; set; }
	}
}

