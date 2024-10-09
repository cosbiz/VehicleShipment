using System.ComponentModel.DataAnnotations;

namespace Domain.DTO.Request
{
    public class UpdateVehicleRequest
    {
        [Required]
        public Guid VehicleId { get; set; }
        public string? VehicleNumber { get; set; }
        public string? VehicleType { get; set; }
        public string? VehicleBrand { get; set; }
        public string? DriverName { get; set; }

        public string? UserId { get; set; }
    }
}
