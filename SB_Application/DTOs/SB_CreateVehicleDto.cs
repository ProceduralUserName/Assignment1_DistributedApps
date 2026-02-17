namespace SB_Application.DTOs
{
    public class SB_CreateVehicleDto
    {
        public string VehicleCode { get; set; } = string.Empty;
        public Guid LocationId { get; set; }
        public string VehicleType { get; set; } = string.Empty;
    }
}
