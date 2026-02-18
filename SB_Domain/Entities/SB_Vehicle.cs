using SB_Domain.Enums;
using SB_Domain.Exceptions;

namespace SB_Domain.Entities
{
    public class SB_Vehicle
    {
        public Guid Id { get; private set; }
        public string VehicleCode { get; private set; } = string.Empty;
        public Guid LocationId { get; private set; }
        public string VehicleType { get; private set; } = string.Empty;
        public SB_VehicleStatus Status { get; private set; }

        private SB_Vehicle() { }

        public SB_Vehicle(Guid id, string vehicleCode, Guid locationId, string vehicleType)
        {
            Id = id;
            VehicleCode = vehicleCode ?? throw new ArgumentNullException(nameof(vehicleCode));
            LocationId = locationId;
            VehicleType = vehicleType ?? throw new ArgumentNullException(nameof(vehicleType));
            Status = SB_VehicleStatus.Available;
        }

        public void MarkAvailable()
        {
            if (Status == SB_VehicleStatus.Reserved)
                throw new SB_InvalidVehicleStateException("Reserved vehicles must be explicitly released.");

            if (Status != SB_VehicleStatus.Rented && Status != SB_VehicleStatus.Serviced)
                throw new SB_InvalidVehicleStateException($"Cannot mark vehicle Available from {Status}.");

            Status = SB_VehicleStatus.Available;
        }

        public void MarkRented()
        {
            if (Status != SB_VehicleStatus.Available)
                throw new SB_InvalidVehicleStateException($"Cannot rent a vehicle that is {Status}.");

            Status = SB_VehicleStatus.Rented;
        }

        public void MarkReserved()
        {
            if (Status != SB_VehicleStatus.Available)
                throw new SB_InvalidVehicleStateException($"Cannot reserve a vehicle that is {Status}.");

            Status = SB_VehicleStatus.Reserved;
        }

        public void MarkServiced()
        {
            if (Status != SB_VehicleStatus.Available)
                throw new SB_InvalidVehicleStateException($"Cannot service a vehicle that is {Status}.");

            Status = SB_VehicleStatus.Serviced;
        }

        public void ReleaseReservation()
        {
            if (Status != SB_VehicleStatus.Reserved)
                throw new SB_InvalidVehicleStateException($"Cannot release reservation for a vehicle that is {Status}.");

            Status = SB_VehicleStatus.Available;
        }
    }
}
