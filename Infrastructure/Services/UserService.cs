using Domain.DTO.Response;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(UserManager<User> userManager, AppDbContext _dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            this._dbContext = _dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        // Get the currently logged-in user
        public async Task<UserResponse?> GetCurrentUserAsync()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            return new UserResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                LicenceNumber = user.LicenceNumber
            };
        }

        // Get user by ID
        public async Task<UserResponse?> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            return new UserResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.UserName,
                LicenceNumber = user.LicenceNumber // Ensure this value is being mapped
            };
        }

        public async Task<List<UserResponse>> GetUsersAsync()
        {
            // Query the users from the AspNetUsers table
            var users = await _dbContext.Users
                .Select(u => new UserResponse
                {
                    Id = u.Id, 
                    FirstName = u.FirstName,
                    LastName = u.LastName
                })
                .ToListAsync();

            return users;
        }

        public async Task<List<UserResponse>> GetUsersWithVehiclesAsync()
        {
            // Fetch users and include vehicle info
            var users = await _dbContext.Users
                .Include(u => u.Vehicle)  // Eager loading vehicles
                .Select(u => new UserResponse
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.UserName,
                    LicenceNumber = u.LicenceNumber,
                    Vehicle = u.Vehicle != null ? new VehicleResponse
                    {
                        VehicleNumber = u.Vehicle.VehicleNumber
                    } : null
                })
                .ToListAsync();

            return users;
        }

        public async Task<BaseResponse> UpdateUserAsync(UserResponse userResponse)
        {
            var user = await _userManager.FindByIdAsync(userResponse.Id);
            if (user == null)
            {
                return new BaseResponse { IsSuccess = false, ErrorMessage = "User not found." };
            }

            // Update user properties
            user.FirstName = userResponse.FirstName;
            user.LastName = userResponse.LastName;
            user.Email = userResponse.Email;
            user.LicenceNumber = userResponse.LicenceNumber;

            // Update the user in the database
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return new BaseResponse { IsSuccess = true };
            }

            return new BaseResponse { IsSuccess = false, ErrorMessage = string.Join(", ", result.Errors.Select(e => e.Description)) };
        }
    }
}
