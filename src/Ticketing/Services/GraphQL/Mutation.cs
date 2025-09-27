using HotChocolate;
using HotChocolate.Authorization;
using Ticketing.Models.Dtos;
using Data.Repository;
using Data.Repository.Dapper;

namespace Ticketing.Services.GraphQL
{
    public class Mutation
    {
        [Authorize(Roles=["SuperAdministrator", "Administrator"])]
        public async Task<int> CreateUser(UserDto userDto, [Service] UsersService service)
        {
            var result = await service.AddAsync(userDto);
            return (int)result;
        }

        public async Task<bool> UpdateUser(UserDto userDto, [Service] UsersService service)
        {
            var result = await service.UpdateAsync(userDto);
            return (bool)result;
        }

        public async Task<RemoveOperationResult> DeleteUser(int id, [Service] UsersService service)
        {
            var result = await service.RemoveAsync(id);
            return (RemoveOperationResult)result;
        }

        [Authorize(Roles=["SuperAdministrator", "Administrator"])]
        public async Task<int> CreateRole(RoleDto roleDto, [Service] RolesService service)
        {
            var result = await service.AddAsync(roleDto);
            return (int)result;
        }

        public async Task<bool> UpdateRole(RoleDto roleDto, [Service] RolesService service)
        {
            var result = await service.UpdateAsync(roleDto);
            return (bool)result;
        }

        public async Task<RemoveOperationResult> DeleteRole(int id, [Service] RolesService service)
        {
            var result = await service.RemoveAsync(id);
            return (RemoveOperationResult)result;
        }

        [Authorize(Roles=["SuperAdministrator", "Administrator"])]
        public async Task<int> CreateUserRole(UserRoleDto userRoleDto, [Service] UserRolesService service)
        {
            var result = await service.AddAsync(userRoleDto);
            return (int)result;
        }

        public async Task<bool> UpdateUserRole(UserRoleDto userRoleDto, [Service] UserRolesService service)
        {
            var result = await service.UpdateAsync(userRoleDto);
            return (bool)result;
        }

        public async Task<RemoveOperationResult> DeleteUserRole(int id, [Service] UserRolesService service)
        {
            var result = await service.RemoveAsync(id);
            return (RemoveOperationResult)result;
        }

        [Authorize(Roles=["SuperAdministrator", "Administrator"])]
        public async Task<int> CreateRoute(RouteDto routeDto, [Service] RoutesService service)
        {
            var result = await service.AddAsync(routeDto);
            return (int)result;
        }

        public async Task<bool> UpdateRoute(RouteDto routeDto, [Service] RoutesService service)
        {
            var result = await service.UpdateAsync(routeDto);
            return (bool)result;
        }

        public async Task<RemoveOperationResult> DeleteRoute(int id, [Service] RoutesService service)
        {
            var result = await service.RemoveAsync(id);
            return (RemoveOperationResult)result;
        }

        [Authorize(Roles=["SuperAdministrator", "Administrator"])]
        public async Task<int> CreateStation(StationDto stationDto, [Service] StationsService service)
        {
            var result = await service.AddAsync(stationDto);
            return (int)result;
        }

        public async Task<bool> UpdateStation(StationDto stationDto, [Service] StationsService service)
        {
            var result = await service.UpdateAsync(stationDto);
            return (bool)result;
        }

        public async Task<RemoveOperationResult> DeleteStation(int id, [Service] StationsService service)
        {
            var result = await service.RemoveAsync(id);
            return (RemoveOperationResult)result;
        }

        [Authorize(Roles=["SuperAdministrator", "Administrator"])]
        public async Task<int> CreateRouteStation(RouteStationDto routeStationDto, [Service] RouteStationsService service)
        {
            var result = await service.AddAsync(routeStationDto);
            return (int)result;
        }

        public async Task<bool> UpdateRouteStation(RouteStationDto routeStationDto, [Service] RouteStationsService service)
        {
            var result = await service.UpdateAsync(routeStationDto);
            return (bool)result;
        }

        public async Task<RemoveOperationResult> DeleteRouteStation(int id, [Service] RouteStationsService service)
        {
            var result = await service.RemoveAsync(id);
            return (RemoveOperationResult)result;
        }

        [Authorize(Roles=["SuperAdministrator", "Administrator"])]
        public async Task<int> CreateTimeSchedule(TimeScheduleDto timeScheduleDto, [Service] TimeSchedulesService service)
        {
            var result = await service.AddAsync(timeScheduleDto);
            return (int)result;
        }

        public async Task<bool> UpdateTimeSchedule(TimeScheduleDto timeScheduleDto, [Service] TimeSchedulesService service)
        {
            var result = await service.UpdateAsync(timeScheduleDto);
            return (bool)result;
        }

        public async Task<RemoveOperationResult> DeleteTimeSchedule(int id, [Service] TimeSchedulesService service)
        {
            var result = await service.RemoveAsync(id);
            return (RemoveOperationResult)result;
        }

        [Authorize(Roles=["SuperAdministrator", "Administrator"])]
        public async Task<int> CreateTrain(TrainDto trainDto, [Service] TrainsService service)
        {
            var result = await service.AddAsync(trainDto);
            return (int)result;
        }

        public async Task<bool> UpdateTrain(TrainDto trainDto, [Service] TrainsService service)
        {
            var result = await service.UpdateAsync(trainDto);
            return (bool)result;
        }

        public async Task<RemoveOperationResult> DeleteTrain(int id, [Service] TrainsService service)
        {
            var result = await service.RemoveAsync(id);
            return (RemoveOperationResult)result;
        }

        [Authorize(Roles=["SuperAdministrator", "Administrator"])]
        public async Task<int> CreateTrainSchedule(TrainScheduleDto trainScheduleDto, [Service] TrainSchedulesService service)
        {
            var result = await service.AddAsync(trainScheduleDto);
            return (int)result;
        }

        public async Task<bool> UpdateTrainSchedule(TrainScheduleDto trainScheduleDto, [Service] TrainSchedulesService service)
        {
            var result = await service.UpdateAsync(trainScheduleDto);
            return (bool)result;
        }

        public async Task<RemoveOperationResult> DeleteTrainSchedule(int id, [Service] TrainSchedulesService service)
        {
            var result = await service.RemoveAsync(id);
            return (RemoveOperationResult)result;
        }

        [Authorize(Roles=["SuperAdministrator", "Administrator"])]
        public async Task<int> CreateTrainWagon(TrainWagonDto trainWagonDto, [Service] TrainWagonsService service)
        {
            var result = await service.AddAsync(trainWagonDto);
            return (int)result;
        }

        public async Task<bool> UpdateTrainWagon(TrainWagonDto trainWagonDto, [Service] TrainWagonsService service)
        {
            var result = await service.UpdateAsync(trainWagonDto);
            return (bool)result;
        }

        public async Task<RemoveOperationResult> DeleteTrainWagon(int id, [Service] TrainWagonsService service)
        {
            var result = await service.RemoveAsync(id);
            return (RemoveOperationResult)result;
        }

        [Authorize(Roles=["SuperAdministrator", "Administrator"])]
        public async Task<int> CreateWagon(WagonDto wagonDto, [Service] WagonsService service)
        {
            var result = await service.AddAsync(wagonDto);
            return (int)result;
        }

        public async Task<bool> UpdateWagon(WagonDto wagonDto, [Service] WagonsService service)
        {
            var result = await service.UpdateAsync(wagonDto);
            return (bool)result;
        }

        public async Task<RemoveOperationResult> DeleteWagon(int id, [Service] WagonsService service)
        {
            var result = await service.RemoveAsync(id);
            return (RemoveOperationResult)result;
        }

        [Authorize(Roles=["SuperAdministrator", "Administrator"])]
        public async Task<int> CreateSeat(SeatDto seatDto, [Service] SeatsService service)
        {
            var result = await service.AddAsync(seatDto);
            return (int)result;
        }

        public async Task<bool> UpdateSeat(SeatDto seatDto, [Service] SeatsService service)
        {
            var result = await service.UpdateAsync(seatDto);
            return (bool)result;
        }

        public async Task<RemoveOperationResult> DeleteSeat(int id, [Service] SeatsService service)
        {
            var result = await service.RemoveAsync(id);
            return (RemoveOperationResult)result;
        }

        [Authorize(Roles=["SuperAdministrator", "Administrator"])]
        public async Task<int> CreateSegment(SegmentDto segmentDto, [Service] SegmentsService service)
        {
            var result = await service.AddAsync(segmentDto);
            return (int)result;
        }

        public async Task<bool> UpdateSegment(SegmentDto segmentDto, [Service] SegmentsService service)
        {
            var result = await service.UpdateAsync(segmentDto);
            return (bool)result;
        }

        public async Task<RemoveOperationResult> DeleteSegment(int id, [Service] SegmentsService service)
        {
            var result = await service.RemoveAsync(id);
            return (RemoveOperationResult)result;
        }

        [Authorize(Roles=["SuperAdministrator", "Administrator"])]
        public async Task<int> CreateSeatReservation(SeatReservationDto seatReservationDto, [Service] SeatReservationsService service)
        {
            var result = await service.AddAsync(seatReservationDto);
            return (int)result;
        }

        public async Task<bool> UpdateSeatReservation(SeatReservationDto seatReservationDto, [Service] SeatReservationsService service)
        {
            var result = await service.UpdateAsync(seatReservationDto);
            return (bool)result;
        }

        public async Task<RemoveOperationResult> DeleteSeatReservation(int id, [Service] SeatReservationsService service)
        {
            var result = await service.RemoveAsync(id);
            return (RemoveOperationResult)result;
        }

        [Authorize(Roles=["SuperAdministrator", "Administrator"])]
        public async Task<int> CreateTicket(TicketDto ticketDto, [Service] TicketsService service)
        {
            var result = await service.AddAsync(ticketDto);
            return (int)result;
        }

        public async Task<bool> UpdateTicket(TicketDto ticketDto, [Service] TicketsService service)
        {
            var result = await service.UpdateAsync(ticketDto);
            return (bool)result;
        }

        public async Task<RemoveOperationResult> DeleteTicket(int id, [Service] TicketsService service)
        {
            var result = await service.RemoveAsync(id);
            return (RemoveOperationResult)result;
        }
    }
}
