namespace Domain.Entities
{
	public class Vehicle
	{
		public Guid Id { get; set; }  // Primary key, using Guid for uniqueness

		public string? VehicleNumber { get; set; }
		public string? VehicleType { get; set; }
		public string? VehicleBrand { get; set; }


        // Nullable UserId to make the relationship optional
        public string? UserId { get; set; }  // Nullable foreign key to User
        public User? User { get; set; }  // Nullable navigation property to User
    }
}