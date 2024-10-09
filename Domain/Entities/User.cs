using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
	public class User : IdentityUser
	{
        public string? Avatar { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? LicenceNumber { get; set; }

        // Nullable Vehicle to indicate a user may or may not have a vehicle
        public Vehicle? Vehicle { get; set; }

        public ICollection<Transport>? Transports { get; set; }
        public bool AccountConfirmed { get; set; }
    }
}
