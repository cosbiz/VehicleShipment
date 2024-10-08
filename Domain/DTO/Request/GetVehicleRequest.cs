namespace Domain.DTO.Request
{
    public class GetVehicleRequest
    {
        public Guid VehicleId { get; set; }
        public string? DriverName { get; set; }
        public string? DriverLicence { get; set; }
    }
}
