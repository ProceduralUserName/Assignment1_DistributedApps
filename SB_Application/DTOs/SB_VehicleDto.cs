namespace SB_Application.DTOs
{
    public class SB_VehicleDto
    {
        public Guid Id { get; set; }
        public string VehicleCode { get; set; } = string.Empty;
        public Guid LocationId { get; set; }
        public string VehicleType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
