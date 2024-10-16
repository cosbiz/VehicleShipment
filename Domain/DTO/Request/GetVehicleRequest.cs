namespace Domain.DTO.Request
{
    public class GetVehicleRequest
    {
        public Guid VehicleId { get; set; }
        public string? DriverName { get; set; }
        public string? DriverLicence { get; set; }
        public string? VehicleType { get; set; }
        public string? VehicleBrand { get; set; }
    }
}
