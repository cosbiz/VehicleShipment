namespace Domain.DTO.Response
{
    public class GetVehicleResponse
    {
        public Guid Id { get; set; }

        public string? VehicleNumber { get; set; }
        public string? VehicleType { get; set; }
        public string? VehicleBrand { get; set; }
        public string? DriverName { get; set; }

        public string? UserId { get; set; }
    }
}
