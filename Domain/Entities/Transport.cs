namespace Domain.Entities
{
	public class Transport
	{
		public Guid Id { get; set; }  // Use Guid as the primary key
		public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public string? EntryPermission { get; set; }
		public string? ExitPermission { get; set; }
        public string? FromFactory { get; set; }
        public string? ToFactory { get; set; }
        public string? InternalLocation { get; set; }
        public string? AdditionalNotes { get; set; }
        public string? LoadType { get; set; }
		public string? LoadWeight { get; set; }

		public required string UserId { get; set; }  // Foreign key to User, also Guid type
		public required User User { get; set; }  // Navigation property to User
	}
}