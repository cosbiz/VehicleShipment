using Domain.DTO.Response;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;

        public UserService(AppDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        // Get user by ID
        public async Task<UserResponse?> GetUserByIdAsync(string userId)
        {
            return await _dbContext.Users
                .Where(u => u.Id == userId)
                .Select(u => new UserResponse
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName
                })
                .FirstOrDefaultAsync();
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
    }
}
