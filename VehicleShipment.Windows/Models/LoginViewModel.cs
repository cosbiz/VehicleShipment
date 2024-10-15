using System.Net.Http.Json;

namespace VehicleShipment.Windows.Models
{
    public class LoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool LoginFailureHidden { get; set; } = true;
    }
}
