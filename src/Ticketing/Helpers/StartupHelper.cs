using Ticketing.Mappings;
using Ticketing.Mappings.Dictionaries;
using Ticketing.Mappings.Tarifications;
using Ticketing.Mappings.Workflows;
using Ticketing.Services;

namespace Ticketing.Helpers
{
    static class StartupHelper
    {
        public static void AddMapping(this WebApplicationBuilder source)
        {
            var services = source.Services;

            services.AddScoped<DbMapContext>();

            services.AddScoped<UserMap>();
            services.AddScoped<RoleMap>();
            services.AddScoped<UserRoleMap>();
            services.AddScoped<RouteMap>();
            services.AddScoped<StationMap>();
            services.AddScoped<RailwayMap>();
            services.AddScoped<RailwayStationMap>();
            services.AddScoped<DepotMap>();
            services.AddScoped<RouteStationMap>();
            services.AddScoped<TrainMap>();
            services.AddScoped<TrainScheduleMap>();
            services.AddScoped<TrainWagonMap>();
            services.AddScoped<TrainWagonsPlanMap>();
            services.AddScoped<TrainWagonsPlanWagonMap>();
            services.AddScoped<WagonModelMap>();
            services.AddScoped<WagonTypeMap>();
            services.AddScoped<WagonFeatureMap>();
            services.AddScoped<WagonModelFeatureMap>();
            services.AddScoped<CarrierMap>();
            services.AddScoped<FilialMap>();
            services.AddScoped<ServiceMap>();
            services.AddScoped<SeatTypeMap>();
            services.AddScoped<SeatPurposeMap>();
            services.AddScoped<SeatMap>();
            services.AddScoped<SeatSegmentMap>();
            services.AddScoped<SeatCountSegmentMap>();
            services.AddScoped<SeatCountReservationMap>();
            services.AddScoped<PeriodicityMap>();
            services.AddScoped<TrainTypeMap>();
            services.AddScoped<ConnectionMap>();
            services.AddScoped<SeatReservationMap>();
            services.AddScoped<TicketMap>();
            services.AddScoped<TicketStateMap>();
            services.AddScoped<TicketServiceMap>();
            services.AddScoped<TicketPaymentMap>();
            services.AddScoped<TrainCategoryMap>();
            services.AddScoped<WagonClassMap>();
            services.AddScoped<SeasonMap>();
            services.AddScoped<BaseFareMap>();
            services.AddScoped<TariffMap>();
            services.AddScoped<TariffTrainCategoryItemMap>();
            services.AddScoped<TariffWagonItemMap>();
            services.AddScoped<TariffWagonTypeItemMap>();
            services.AddScoped<TariffSeatTypeItemMap>();
            services.AddScoped<SeatTariffMap>();
            services.AddScoped<SeatTariffItemMap>();
            services.AddScoped<SeatTariffHistoryMap>();
            services.AddScoped<WorkflowTaskMap>();
            services.AddScoped<WorkflowTaskProgressMap>();
            services.AddScoped<WorkflowTaskLogMap>();
        }

        public static void AddServices(this WebApplicationBuilder source)
        {
            var services = source.Services;

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();

            services.AddScoped<SeatReservationsService>();
            services.AddScoped<TrainSchedulesService>();
            services.AddScoped<TrainWagonsService>();
            services.AddScoped<WagonModelsService>();
            services.AddScoped<WorkflowTaskService>();

        }

        public static void AddProviders(this WebApplicationBuilder source)
        {
            var services = source.Services;

        }
    }
}
