using SB_Domain.Entities;
using SB_Domain.Enums;
using SB_Domain.Exceptions;
using SB_Domain.ValueObjects;

namespace SB_Domain.Tests
{
    public class SB_VehicleTransitionTests
    {
        private SB_Vehicle CreateVehicleWithStatus(SB_VehicleStatus status)
        {
            var vehicle = new SB_Vehicle(
                Guid.NewGuid(),
                new SB_VehicleCode("TEST-001"),
                Guid.NewGuid(),
                new SB_VehicleType("Sedan"));

            switch (status)
            {
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

            return vehicle;
        }

        // MarkRented tests

        [Fact]
        public void MarkRented_FromAvailable_Succeeds()
        {
            var vehicle = CreateVehicleWithStatus(SB_VehicleStatus.Available);
            vehicle.MarkRented();
            Assert.Equal(SB_VehicleStatus.Rented, vehicle.Status);
        }

        [Fact]
        public void MarkRented_FromRented_Throws()
        {
            var vehicle = CreateVehicleWithStatus(SB_VehicleStatus.Rented);
            Assert.Throws<SB_InvalidVehicleStateException>(() => vehicle.MarkRented());
        }

        [Fact]
        public void MarkRented_FromReserved_Throws()
        {
            var vehicle = CreateVehicleWithStatus(SB_VehicleStatus.Reserved);
            Assert.Throws<SB_InvalidVehicleStateException>(() => vehicle.MarkRented());
        }

        [Fact]
        public void MarkRented_FromServiced_Throws()
        {
            var vehicle = CreateVehicleWithStatus(SB_VehicleStatus.Serviced);
            Assert.Throws<SB_InvalidVehicleStateException>(() => vehicle.MarkRented());
        }

        // MarkReserved tests

        [Fact]
        public void MarkReserved_FromAvailable_Succeeds()
        {
            var vehicle = CreateVehicleWithStatus(SB_VehicleStatus.Available);
            vehicle.MarkReserved();
            Assert.Equal(SB_VehicleStatus.Reserved, vehicle.Status);
        }

        [Fact]
        public void MarkReserved_FromNonAvailable_Throws()
        {
            var vehicle = CreateVehicleWithStatus(SB_VehicleStatus.Rented);
            Assert.Throws<SB_InvalidVehicleStateException>(() => vehicle.MarkReserved());
        }

        // MarkServiced tests

        [Fact]
        public void MarkServiced_FromAvailable_Succeeds()
        {
            var vehicle = CreateVehicleWithStatus(SB_VehicleStatus.Available);
            vehicle.MarkServiced();
            Assert.Equal(SB_VehicleStatus.Serviced, vehicle.Status);
        }

        [Fact]
        public void MarkServiced_FromNonAvailable_Throws()
        {
            var vehicle = CreateVehicleWithStatus(SB_VehicleStatus.Rented);
            Assert.Throws<SB_InvalidVehicleStateException>(() => vehicle.MarkServiced());
        }

        // MarkAvailable tests

        [Fact]
        public void MarkAvailable_FromRented_Succeeds()
        {
            var vehicle = CreateVehicleWithStatus(SB_VehicleStatus.Rented);
            vehicle.MarkAvailable();
            Assert.Equal(SB_VehicleStatus.Available, vehicle.Status);
        }

        [Fact]
        public void MarkAvailable_FromServiced_Succeeds()
        {
            var vehicle = CreateVehicleWithStatus(SB_VehicleStatus.Serviced);
            vehicle.MarkAvailable();
            Assert.Equal(SB_VehicleStatus.Available, vehicle.Status);
        }

        [Fact]
        public void MarkAvailable_FromReserved_Throws()
        {
            var vehicle = CreateVehicleWithStatus(SB_VehicleStatus.Reserved);
            Assert.Throws<SB_InvalidVehicleStateException>(() => vehicle.MarkAvailable());
        }

        [Fact]
        public void MarkAvailable_FromAvailable_Throws()
        {
            var vehicle = CreateVehicleWithStatus(SB_VehicleStatus.Available);
            Assert.Throws<SB_InvalidVehicleStateException>(() => vehicle.MarkAvailable());
        }

        // ReleaseReservation tests

        [Fact]
        public void ReleaseReservation_FromReserved_Succeeds()
        {
            var vehicle = CreateVehicleWithStatus(SB_VehicleStatus.Reserved);
            vehicle.ReleaseReservation();
            Assert.Equal(SB_VehicleStatus.Available, vehicle.Status);
        }

        [Fact]
        public void ReleaseReservation_FromNonReserved_Throws()
        {
            var vehicle = CreateVehicleWithStatus(SB_VehicleStatus.Available);
            Assert.Throws<SB_InvalidVehicleStateException>(() => vehicle.ReleaseReservation());
        }

        // TransitionTo tests

        [Fact]
        public void TransitionTo_Available_FromReserved_UsesReleaseReservation()
        {
            var vehicle = CreateVehicleWithStatus(SB_VehicleStatus.Reserved);
            vehicle.TransitionTo(SB_VehicleStatus.Available);
            Assert.Equal(SB_VehicleStatus.Available, vehicle.Status);
        }

        [Fact]
        public void TransitionTo_Available_FromRented_UsesMarkAvailable()
        {
            var vehicle = CreateVehicleWithStatus(SB_VehicleStatus.Rented);
            vehicle.TransitionTo(SB_VehicleStatus.Available);
            Assert.Equal(SB_VehicleStatus.Available, vehicle.Status);
        }

        [Fact]
        public void TransitionTo_Rented_FromAvailable_Succeeds()
        {
            var vehicle = CreateVehicleWithStatus(SB_VehicleStatus.Available);
            vehicle.TransitionTo(SB_VehicleStatus.Rented);
            Assert.Equal(SB_VehicleStatus.Rented, vehicle.Status);
        }
    }
}
