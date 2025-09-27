using HotChocolate;
using HotChocolate.Authorization;
using Ticketing.Models.Dtos;

namespace Ticketing.Services.GraphQL
{
    public class Query
    {
        [Authorize(Roles=["SuperAdministrator", "Administrator"])]
        public async Task<PagedList<UserDto>> Users(UserQuery query, [GlobalState("currentUser")] ClaimsPrincipal user, [Service] UsersService service)
        {
            return await service.SearchAsync(query);
        }

        [Authorize(Roles=["SuperAdministrator", "Administrator"])]
        public async Task<PagedList<RoleDto>> Roles(RoleQuery query, [GlobalState("currentUser")] ClaimsPrincipal user, [Service] RolesService service)
        {
            return await service.SearchAsync(query);
        }

        [Authorize(Roles=["SuperAdministrator", "Administrator"])]
        public async Task<PagedList<UserRoleDto>> UserRoles(UserRoleQuery query, [GlobalState("currentUser")] ClaimsPrincipal user, [Service] UserRolesService service)
        {
            return await service.SearchAsync(query);
        }

        [Authorize(Roles=["SuperAdministrator", "Administrator"])]
        public async Task<PagedList<RouteDto>> Routes(RouteQuery query, [GlobalState("currentUser")] ClaimsPrincipal user, [Service] RoutesService service)
        {
            return await service.SearchAsync(query);
        }

        [Authorize(Roles=["SuperAdministrator", "Administrator"])]
        public async Task<PagedList<StationDto>> Stations(StationQuery query, [GlobalState("currentUser")] ClaimsPrincipal user, [Service] StationsService service)
        {
            return await service.SearchAsync(query);
        }

        [Authorize(Roles=["SuperAdministrator", "Administrator"])]
        public async Task<PagedList<RouteStationDto>> RouteStations(RouteStationQuery query, [GlobalState("currentUser")] ClaimsPrincipal user, [Service] RouteStationsService service)
        {
            return await service.SearchAsync(query);
        }

        [Authorize(Roles=["SuperAdministrator", "Administrator"])]
        public async Task<PagedList<TimeScheduleDto>> TimeSchedules(TimeScheduleQuery query, [GlobalState("currentUser")] ClaimsPrincipal user, [Service] TimeSchedulesService service)
        {
            return await service.SearchAsync(query);
        }

        [Authorize(Roles=["SuperAdministrator", "Administrator"])]
        public async Task<PagedList<TrainDto>> Trains(TrainQuery query, [GlobalState("currentUser")] ClaimsPrincipal user, [Service] TrainsService service)
        {
            return await service.SearchAsync(query);
        }

        [Authorize(Roles=["SuperAdministrator", "Administrator"])]
        public async Task<PagedList<TrainScheduleDto>> TrainSchedules(TrainScheduleQuery query, [GlobalState("currentUser")] ClaimsPrincipal user, [Service] TrainSchedulesService service)
        {
            return await service.SearchAsync(query);
        }

        [Authorize(Roles=["SuperAdministrator", "Administrator"])]
        public async Task<PagedList<TrainWagonDto>> TrainWagons(TrainWagonQuery query, [GlobalState("currentUser")] ClaimsPrincipal user, [Service] TrainWagonsService service)
        {
            return await service.SearchAsync(query);
        }

        [Authorize(Roles=["SuperAdministrator", "Administrator"])]
        public async Task<PagedList<WagonDto>> Wagons(WagonQuery query, [GlobalState("currentUser")] ClaimsPrincipal user, [Service] WagonsService service)
        {
            return await service.SearchAsync(query);
        }

        [Authorize(Roles=["SuperAdministrator", "Administrator"])]
        public async Task<PagedList<SeatDto>> Seats(SeatQuery query, [GlobalState("currentUser")] ClaimsPrincipal user, [Service] SeatsService service)
        {
            return await service.SearchAsync(query);
        }

        [Authorize(Roles=["SuperAdministrator", "Administrator"])]
        public async Task<PagedList<SegmentDto>> Segments(SegmentQuery query, [GlobalState("currentUser")] ClaimsPrincipal user, [Service] SegmentsService service)
        {
            return await service.SearchAsync(query);
        }

        [Authorize(Roles=["SuperAdministrator", "Administrator"])]
        public async Task<PagedList<SeatReservationDto>> SeatReservations(SeatReservationQuery query, [GlobalState("currentUser")] ClaimsPrincipal user, [Service] SeatReservationsService service)
        {
            return await service.SearchAsync(query);
        }

        [Authorize(Roles=["SuperAdministrator", "Administrator"])]
        public async Task<PagedList<TicketDto>> Tickets(TicketQuery query, [GlobalState("currentUser")] ClaimsPrincipal user, [Service] TicketsService service)
        {
            return await service.SearchAsync(query);
        }
    }
}
