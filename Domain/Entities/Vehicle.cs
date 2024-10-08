namespace Domain.Entities
{
	public class Vehicle
	{
		public Guid Id { get; set; }  // Primary key, using Guid for uniqueness

		public string? VehicleNumber { get; set; }
		public string? VehicleType { get; set; }
		public string? VehicleBrand { get; set; }

		public required string UserId { get; set; }  // Foreign key to the User
		public required User User { get; set; }  // Navigation property to User
	}
}