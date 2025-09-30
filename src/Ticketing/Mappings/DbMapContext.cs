using Ticketing.Mappings.Tarifications;

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
        public WagonMap WagonMap { get; }
        public WagonTypeMap WagonTypeMap { get; }
        public ServiceMap ServiceMap { get; }
        public SeatTypeMap SeatTypeMap { get; }
        public SeatMap SeatMap { get; }
        public SeatSegmentMap SeatSegmentMap { get; }
        public SeatCountSegmentMap SeatCountSegmentMap { get; }
        public SeatCountReservationMap SeatCountReservationMap { get; }
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
        public SeatTariffMap SeatTariffMap { get; }
        public SeatTariffItemMap SeatTariffItemMap { get; }
        public SeatTariffHistoryMap SeatTariffHistoryMap { get; }

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
            WagonMap = new WagonMap(this);
            WagonTypeMap = new WagonTypeMap(this);
            ServiceMap = new ServiceMap(this);
            SeatTypeMap = new SeatTypeMap(this);
            SeatMap = new SeatMap(this);
            SeatSegmentMap = new SeatSegmentMap(this);
            SeatCountSegmentMap = new SeatCountSegmentMap(this);
            SeatCountReservationMap = new SeatCountReservationMap(this);
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
            SeatTariffMap = new SeatTariffMap(this);
            SeatTariffItemMap = new SeatTariffItemMap(this);
            SeatTariffHistoryMap = new SeatTariffHistoryMap(this);
        }
    }
}
