using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace VehicleShipment.Windows.Services
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        public CustomAuthenticationStateProvider()
        {
        }

        public async Task Login(string token)
        {
            await SecureStorage.SetAsync("accounttoken", token);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task Logout()
        {
            SecureStorage.Remove("accounttoken");
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            try
            {
                var userInfo = await SecureStorage.GetAsync("accounttoken");
                if (userInfo != null)
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, "ffUser") };
                    identity = new ClaimsIdentity(claims, "Server authentication");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Request failed:" + ex.ToString());
            }

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
    }
}
