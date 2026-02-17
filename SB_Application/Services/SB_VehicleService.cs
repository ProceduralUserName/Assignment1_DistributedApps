using SB_Application.DTOs;
using SB_Application.Interfaces;
using SB_Domain.Entities;
using SB_Domain.Enums;

namespace SB_Application.Services
{
    public class SB_VehicleService
    {
        private readonly ISB_VehicleRepository _repository;

        public SB_VehicleService(ISB_VehicleRepository repository)
        {
            _repository = repository;
        }

        public async Task<SB_VehicleDto> CreateVehicleAsync(SB_CreateVehicleDto dto)
        {
            var vehicle = new SB_Vehicle(Guid.NewGuid(), dto.VehicleCode, dto.LocationId, dto.VehicleType);
            await _repository.AddAsync(vehicle);
            return MapToDto(vehicle);
        }

        public async Task<SB_VehicleDto?> GetVehicleByIdAsync(Guid id)
        {
            var vehicle = await _repository.GetByIdAsync(id);
            return vehicle == null ? null : MapToDto(vehicle);
        }

        public async Task<IEnumerable<SB_VehicleDto>> GetAllVehiclesAsync()
        {
            var vehicles = await _repository.GetAllAsync();
            return vehicles.Select(MapToDto);
        }

        public async Task<SB_VehicleDto> UpdateVehicleStatusAsync(Guid id, SB_UpdateVehicleStatusDto dto)
        {
            var vehicle = await _repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Vehicle with id {id} not found.");

            var status = Enum.Parse<SB_VehicleStatus>(dto.Status, ignoreCase: true);

            switch (status)
            {
                case SB_VehicleStatus.Available:
                    if (vehicle.Status == SB_VehicleStatus.Reserved)
                        vehicle.ReleaseReservation();
                    else
                        vehicle.MarkAvailable();
                    break;
                case SB_VehicleStatus.Rented:
                    vehicle.MarkRented();
                    break;
                case SB_VehicleStatus.Reserved:
                    vehicle.MarkReserved();
                    break;
                case SB_VehicleStatus.Serviced:
                    vehicle.MarkServiced();
                    break;
            }

            await _repository.UpdateAsync(vehicle);
            return MapToDto(vehicle);
        }

        public async Task DeleteVehicleAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        private static SB_VehicleDto MapToDto(SB_Vehicle vehicle)
        {
            return new SB_VehicleDto
            {
                Id = vehicle.Id,
                VehicleCode = vehicle.VehicleCode,
                LocationId = vehicle.LocationId,
                VehicleType = vehicle.VehicleType,
                Status = vehicle.Status.ToString()
            };
        }
    }
}
