using Ticketing.Mappings.Dictionaries;
using Ticketing.Mappings.Tarifications;
using Ticketing.Mappings.Workflows;

namespace Ticketing.Mappings
{
    public partial class DbMapContext
    {
        public UserMap UserMap { get; }
        public RoleMap RoleMap { get; }
        public UserRoleMap UserRoleMap { get; }
        public RouteMap RouteMap { get; }
        public StationMap StationMap { get; }
        public RailwayMap RailwayMap { get; }
        public RailwayStationMap RailwayStationMap { get; }
        public DepotMap DepotMap { get; }
        public RouteStationMap RouteStationMap { get; }
        public TrainMap TrainMap { get; }
        public TrainScheduleMap TrainScheduleMap { get; }
        public TrainWagonMap TrainWagonMap { get; }
        public TrainWagonsPlanMap TrainWagonsPlanMap { get; }
        public TrainWagonsPlanWagonMap TrainWagonsPlanWagonMap { get; }
        public WagonModelMap WagonModelMap { get; }
        public WagonTypeMap WagonTypeMap { get; }
        public WagonFeatureMap WagonFeatureMap { get; }
        public WagonModelFeatureMap WagonModelFeatureMap { get; }
        public CarrierMap CarrierMap { get; }
        public FilialMap FilialMap { get; }
        public ServiceMap ServiceMap { get; }
        public SeatTypeMap SeatTypeMap { get; }
        public SeatPurposeMap SeatPurposeMap { get; }
        public SeatMap SeatMap { get; }
        public SeatSegmentMap SeatSegmentMap { get; }
        public SeatCountSegmentMap SeatCountSegmentMap { get; }
        public SeatCountReservationMap SeatCountReservationMap { get; }
        public PeriodicityMap PeriodicityMap { get; }
        public TrainTypeMap TrainTypeMap { get; }
        public ConnectionMap ConnectionMap { get; }
        public SeatReservationMap SeatReservationMap { get; }
        public TicketMap TicketMap { get; }
        public TicketStateMap TicketStateMap { get; }
        public TicketServiceMap TicketServiceMap { get; }
        public TicketPaymentMap TicketPaymentMap { get; }
        public TrainCategoryMap TrainCategoryMap { get; }
        public WagonClassMap WagonClassMap { get; }
        public SeasonMap SeasonMap { get; }
        public BaseFareMap BaseFareMap { get; }
        public TariffMap TariffMap { get; }
        public TariffTrainCategoryItemMap TariffTrainCategoryItemMap { get; }
        public TariffWagonItemMap TariffWagonItemMap { get; }
        public TariffWagonTypeItemMap TariffWagonTypeItemMap { get; }
        public TariffSeatTypeItemMap TariffSeatTypeItemMap { get; }
        public SeatTariffMap SeatTariffMap { get; }
        public SeatTariffItemMap SeatTariffItemMap { get; }
        public SeatTariffHistoryMap SeatTariffHistoryMap { get; }
        public WorkflowTaskMap WorkflowTaskMap { get; }
        public WorkflowTaskProgressMap WorkflowTaskProgressMap { get; }
        public WorkflowTaskLogMap WorkflowTaskLogMap { get; }

        public DbMapContext()
        {
            UserMap = new UserMap(this);
            RoleMap = new RoleMap(this);
            UserRoleMap = new UserRoleMap(this);
            RouteMap = new RouteMap(this);
            StationMap = new StationMap(this);
            RailwayMap = new RailwayMap(this);
            RailwayStationMap = new RailwayStationMap(this);
            DepotMap = new DepotMap(this);
            RouteStationMap = new RouteStationMap(this);
            TrainMap = new TrainMap(this);
            TrainScheduleMap = new TrainScheduleMap(this);
            TrainWagonMap = new TrainWagonMap(this);
            TrainWagonsPlanMap = new TrainWagonsPlanMap(this);
            TrainWagonsPlanWagonMap = new TrainWagonsPlanWagonMap(this);
            WagonModelMap = new WagonModelMap(this);
            WagonTypeMap = new WagonTypeMap(this);
            WagonFeatureMap = new WagonFeatureMap(this);
            WagonModelFeatureMap = new WagonModelFeatureMap(this);
            CarrierMap = new CarrierMap(this);
            FilialMap = new FilialMap(this);
            ServiceMap = new ServiceMap(this);
            SeatTypeMap = new SeatTypeMap(this);
            SeatPurposeMap = new SeatPurposeMap(this);
            SeatMap = new SeatMap(this);
            SeatSegmentMap = new SeatSegmentMap(this);
            SeatCountSegmentMap = new SeatCountSegmentMap(this);
            SeatCountReservationMap = new SeatCountReservationMap(this);
            PeriodicityMap = new PeriodicityMap(this);
            TrainTypeMap = new TrainTypeMap(this);
            ConnectionMap = new ConnectionMap(this);
            SeatReservationMap = new SeatReservationMap(this);
            TicketMap = new TicketMap(this);
            TicketStateMap = new TicketStateMap(this);
            TicketServiceMap = new TicketServiceMap(this);
            TicketPaymentMap = new TicketPaymentMap(this);
            TrainCategoryMap = new TrainCategoryMap(this);
            WagonClassMap = new WagonClassMap(this);
            SeasonMap = new SeasonMap(this);
            BaseFareMap = new BaseFareMap(this);
            TariffMap = new TariffMap(this);
            TariffTrainCategoryItemMap = new TariffTrainCategoryItemMap(this);
            TariffWagonItemMap = new TariffWagonItemMap(this);
            TariffWagonTypeItemMap = new TariffWagonTypeItemMap(this);
            TariffSeatTypeItemMap = new TariffSeatTypeItemMap(this);
            SeatTariffMap = new SeatTariffMap(this);
            SeatTariffItemMap = new SeatTariffItemMap(this);
            SeatTariffHistoryMap = new SeatTariffHistoryMap(this);
            WorkflowTaskMap = new WorkflowTaskMap(this);
            WorkflowTaskProgressMap = new WorkflowTaskProgressMap(this);
            WorkflowTaskLogMap = new WorkflowTaskLogMap(this);
        }
    }
}
