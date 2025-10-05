using Data.Repository;
using Ticketing.Data.TicketDb.Entities;
using Ticketing.Data.TicketDb.Entities.Tarifications;
using Ticketing.Data.TicketDb.Entities.Workflows;
using Microsoft.EntityFrameworkCore;

namespace Ticketing.Data.TicketDb.DatabaseContext
{
    public partial class TicketDbContext : DbContext
    {
        public DbSet<User>? Users { get; set; }
        public DbSet<Role>? Roles { get; set; }
        public DbSet<UserRole>? UserRoles { get; set; }
        public DbSet<Entities.Route>? Routes { get; set; }
        public DbSet<Station>? Stations { get; set; }
        public DbSet<Railway>? Railwaies { get; set; }
        public DbSet<RailwayStation>? RailwayStations { get; set; }
        public DbSet<Depot>? Depots { get; set; }
        public DbSet<RouteStation>? RouteStations { get; set; }
        public DbSet<Train>? Trains { get; set; }
        public DbSet<TrainSchedule>? TrainSchedules { get; set; }
        public DbSet<TrainWagon>? TrainWagons { get; set; }
        public DbSet<TrainWagonsPlan>? TrainWagonsPlans { get; set; }
        public DbSet<TrainWagonsPlanWagon>? TrainWagonsPlanWagons { get; set; }
        public DbSet<WagonModel>? WagonModels { get; set; }
        public DbSet<WagonType>? WagonTypes { get; set; }
        public DbSet<Carrier>? Carriers { get; set; }
        public DbSet<Service>? Services { get; set; }
        public DbSet<SeatType>? SeatTypes { get; set; }
        public DbSet<Seat>? Seats { get; set; }
        public DbSet<SeatSegment>? SeatSegments { get; set; }
        public DbSet<SeatCountSegment>? SeatCountSegments { get; set; }
        public DbSet<SeatCountReservation>? SeatCountReservations { get; set; }
        public DbSet<Connection>? Connections { get; set; }
        public DbSet<SeatReservation>? SeatReservations { get; set; }
        public DbSet<Ticket>? Tickets { get; set; }
        public DbSet<TicketState>? TicketStates { get; set; }
        public DbSet<TicketService>? TicketServices { get; set; }
        public DbSet<TicketPayment>? TicketPayments { get; set; }
        public DbSet<TrainCategory>? TrainCategories { get; set; }
        public DbSet<WagonClass>? WagonClasses { get; set; }
        public DbSet<Season>? Seasons { get; set; }
        public DbSet<BaseFare>? BaseFares { get; set; }
        public DbSet<Tariff>? Tariffs { get; set; }
        public DbSet<TariffTrainCategoryItem>? TariffTrainCategoryItems { get; set; }
        public DbSet<TariffWagonItem>? TariffWagonItems { get; set; }
        public DbSet<TariffWagonTypeItem>? TariffWagonTypeItems { get; set; }
        public DbSet<TariffSeatTypeItem>? TariffSeatTypeItems { get; set; }
        public DbSet<SeatTariff>? SeatTariffs { get; set; }
        public DbSet<SeatTariffItem>? SeatTariffItems { get; set; }
        public DbSet<SeatTariffHistory>? SeatTariffHistories { get; set; }
        public DbSet<WorkflowTask>? WorkflowTasks { get; set; }
        public DbSet<WorkflowTaskProgress>? WorkflowTaskProgresses { get; set; }
        public DbSet<WorkflowTaskLog>? WorkflowTaskLogs { get; set; }

        public TicketDbContext(DbContextOptions<TicketDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsersConfiguration { IsInMemoryDb = this.IsInMemoryDb() });
            modelBuilder.ApplyConfiguration(new RolesConfiguration());
            modelBuilder.ApplyConfiguration(new UserRolesConfiguration());
            modelBuilder.ApplyConfiguration(new RoutesConfiguration());
            modelBuilder.ApplyConfiguration(new StationsConfiguration { IsInMemoryDb = this.IsInMemoryDb() });
            modelBuilder.ApplyConfiguration(new RailwaiesConfiguration());
            modelBuilder.ApplyConfiguration(new RailwayStationsConfiguration());
            modelBuilder.ApplyConfiguration(new DepotsConfiguration());
            modelBuilder.ApplyConfiguration(new RouteStationsConfiguration());
            modelBuilder.ApplyConfiguration(new TrainsConfiguration());
            modelBuilder.ApplyConfiguration(new TrainSchedulesConfiguration());
            modelBuilder.ApplyConfiguration(new TrainWagonsConfiguration());
            modelBuilder.ApplyConfiguration(new TrainWagonsPlansConfiguration());
            modelBuilder.ApplyConfiguration(new TrainWagonsPlanWagonsConfiguration());
            modelBuilder.ApplyConfiguration(new WagonModelsConfiguration { IsInMemoryDb = this.IsInMemoryDb() });
            modelBuilder.ApplyConfiguration(new WagonTypesConfiguration());
            modelBuilder.ApplyConfiguration(new CarriersConfiguration { IsInMemoryDb = this.IsInMemoryDb() });
            modelBuilder.ApplyConfiguration(new ServicesConfiguration());
            modelBuilder.ApplyConfiguration(new SeatTypesConfiguration());
            modelBuilder.ApplyConfiguration(new SeatsConfiguration());
            modelBuilder.ApplyConfiguration(new SeatSegmentsConfiguration());
            modelBuilder.ApplyConfiguration(new SeatCountSegmentsConfiguration { IsInMemoryDb = this.IsInMemoryDb() });
            modelBuilder.ApplyConfiguration(new SeatCountReservationsConfiguration());
            modelBuilder.ApplyConfiguration(new ConnectionsConfiguration());
            modelBuilder.ApplyConfiguration(new SeatReservationsConfiguration());
            modelBuilder.ApplyConfiguration(new TicketsConfiguration());
            modelBuilder.ApplyConfiguration(new TicketStatesConfiguration());
            modelBuilder.ApplyConfiguration(new TicketServicesConfiguration());
            modelBuilder.ApplyConfiguration(new TicketPaymentsConfiguration());
            modelBuilder.ApplyConfiguration(new TrainCategoriesConfiguration());
            modelBuilder.ApplyConfiguration(new WagonClassesConfiguration());
            modelBuilder.ApplyConfiguration(new SeasonsConfiguration());
            modelBuilder.ApplyConfiguration(new BaseFaresConfiguration());
            modelBuilder.ApplyConfiguration(new TariffsConfiguration());
            modelBuilder.ApplyConfiguration(new TariffTrainCategoryItemsConfiguration());
            modelBuilder.ApplyConfiguration(new TariffWagonItemsConfiguration());
            modelBuilder.ApplyConfiguration(new TariffWagonTypeItemsConfiguration());
            modelBuilder.ApplyConfiguration(new TariffSeatTypeItemsConfiguration());
            modelBuilder.ApplyConfiguration(new SeatTariffsConfiguration());
            modelBuilder.ApplyConfiguration(new SeatTariffItemsConfiguration { IsInMemoryDb = this.IsInMemoryDb() });
            modelBuilder.ApplyConfiguration(new SeatTariffHistoriesConfiguration());
            modelBuilder.ApplyConfiguration(new WorkflowTasksConfiguration { IsInMemoryDb = this.IsInMemoryDb() });
            modelBuilder.ApplyConfiguration(new WorkflowTaskProgressesConfiguration { IsInMemoryDb = this.IsInMemoryDb() });
            modelBuilder.ApplyConfiguration(new WorkflowTaskLogsConfiguration { IsInMemoryDb = this.IsInMemoryDb() });
        }
    }
}
