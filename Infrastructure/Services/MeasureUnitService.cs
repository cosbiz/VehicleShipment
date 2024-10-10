using Domain.DTO.Request;
using Domain.DTO.Response;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class MeasureUnitService : IMeasureUnitService
    {
        private readonly AppDbContext _dbContext;

        public MeasureUnitService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
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
                .Select(mu => new MeasureUnitResponse
                {
                    Id = mu.Id,
                    Name = mu.Name,
                    Code = mu.Code
                })
                .ToListAsync();
        }

        // Get a specific measure unit by ID, returning MeasureUnitResponse
        public async Task<MeasureUnitResponse?> GetMesureUnitByIdAsync(int id)
        {
            var measureUnit = await _dbContext.MesureUnits.FindAsync(id);
            if (measureUnit == null)
            {
                return null;
            }

            return new MeasureUnitResponse
            {
                Id = measureUnit.Id,
                Name = measureUnit.Name,
                Code = measureUnit.Code
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
            var user = await _dbContext.Users.FindAsync(measureUnitRequest.UserId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            // Update properties of the existing measure unit
            existingMeasureUnit.Name = measureUnitRequest.Name;
            existingMeasureUnit.Code = measureUnitRequest.Code;
            existingMeasureUnit.UserId = measureUnitRequest.UserId;  // Update the User who modified it

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
