namespace Domain.DTO.Response
{
    public class UserResponse
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? LicenceNumber { get; set; }

        // Nullable vehicle information
        public VehicleResponse? Vehicle { get; set; }
    }

    public class VehicleResponse
    {
        public string VehicleNumber { get; set; }
    }
}
