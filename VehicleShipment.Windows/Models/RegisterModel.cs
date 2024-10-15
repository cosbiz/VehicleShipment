using System.ComponentModel.DataAnnotations;

namespace VehicleShipment.Windows.Models
{
    public class RegisterModel
    {
        [Required]
        public string FirstName { get; set; } = "";

        [Required]
        public string LastName { get; set; } = "";

        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        public string? LicenceNumber { get; set; }  // Optional license number
    }
}
