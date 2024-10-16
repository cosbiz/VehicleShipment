using Domain.DTO.Request;
using Domain.DTO.Response;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Infrastructure.Services
{
    public class MeasureUnitService : IMeasureUnitService
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public MeasureUnitService(AppDbContext dbContext, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, AuthenticationStateProvider authenticationStateProvider)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _authenticationStateProvider = authenticationStateProvider;
        }

        // Create a new measure unit using MeasureUnitRequest and return MeasureUnitResponse after creation
        public async Task<MeasureUnitResponse> CreateMesureUnitAsync(MesureUnitRequest measureUnitRequest)
        {
            // Validate User existence
            var user = await _dbContext.Users.FindAsync(measureUnitRequest.UserId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            var newMeasureUnit = new MesureUnit
            {
                Name = measureUnitRequest.Name,
                Code = measureUnitRequest.Code,
                UserId = measureUnitRequest.UserId // Associate the measure unit with the user
            };

            _dbContext.MesureUnits.Add(newMeasureUnit);
            await _dbContext.SaveChangesAsync();

            return new MeasureUnitResponse
            {
                Id = newMeasureUnit.Id,
                Name = newMeasureUnit.Name,
                Code = newMeasureUnit.Code
            };
        }

        // Delete a measure unit by ID
        public async Task<bool> DeleteMesureUnitAsync(int id)
        {
            var measureUnit = await _dbContext.MesureUnits.FindAsync(id);
            if (measureUnit == null)
            {
                throw new KeyNotFoundException("Measure unit not found.");
            }

            _dbContext.MesureUnits.Remove(measureUnit);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        // Get all measure units, returning a list of MeasureUnitResponse
        public async Task<List<MeasureUnitResponse>> GetAllMesureUnitsAsync()
        {
            return await _dbContext.MesureUnits
                .Include(mu => mu.User)  // Include the user who created it
                .Select(mu => new MeasureUnitResponse
                {
                    Id = mu.Id,
                    Name = mu.Name,
                    Code = mu.Code,
                    FirstName = mu.User.FirstName,
                    LastName = mu.User.LastName,
                })
                .ToListAsync();
        }

        // Get a specific measure unit by ID, returning MeasureUnitResponse
        public async Task<MeasureUnitResponse?> GetMesureUnitByIdAsync(int id)
        {
            var measureUnit = await _dbContext.MesureUnits
        .Include(mu => mu.User)  // Include user data if necessary
        .FirstOrDefaultAsync(mu => mu.Id == id);

            if (measureUnit == null)
            {
                return null;
            }

            return new MeasureUnitResponse
            {
                Id = measureUnit.Id,
                Name = measureUnit.Name,
                Code = measureUnit.Code,
                FirstName = measureUnit.User.FirstName,
                LastName = measureUnit.User.LastName,
            };
        }

        // Update an existing measure unit using MeasureUnitRequest and return MeasureUnitResponse
        public async Task<MeasureUnitResponse> UpdateMesureUnitAsync(int id, MesureUnitRequest measureUnitRequest)
        {
            var existingMeasureUnit = await _dbContext.MesureUnits.FindAsync(id);
            if (existingMeasureUnit == null)
            {
                throw new KeyNotFoundException("Measure unit not found.");
            }

            // Validate User existence


            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
                var userState = authState.User;

                if (userState.Identity != null && userState.Identity.IsAuthenticated)
                {
                    userId = userState.FindFirst(c => c.Type == "sub")?.Value;
                }
                else
                {
                    return null;
                }
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            // Update properties of the existing measure unit
            existingMeasureUnit.Name = measureUnitRequest.Name;
            existingMeasureUnit.Code = measureUnitRequest.Code;
            existingMeasureUnit.UserId = user.Id;  // Update the User who modified it

            _dbContext.MesureUnits.Update(existingMeasureUnit);
            await _dbContext.SaveChangesAsync();

            return new MeasureUnitResponse
            {
                Id = existingMeasureUnit.Id,
                Name = existingMeasureUnit.Name,
                Code = existingMeasureUnit.Code
            };
        }
    }
}
