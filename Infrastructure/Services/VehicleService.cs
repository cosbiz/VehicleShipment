using Domain.DTO.Request;
using Domain.DTO.Response;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly AppDbContext _dbContext;

        public VehicleService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Fetch all vehicles asynchronously
        public async Task<List<Vehicle>> GetAllVehiclesAsync()
        {
            return await _dbContext.Vehicles.ToListAsync();  // Assuming Vehicles is the DbSet for Vehicle entity
        }

        // Get a list of vehicles based on some request criteria (example implementation)
        public List<GetVehicleResponse> GetVehicles(GetVehicleRequest request)
        {
            // Filter vehicles based on request properties (e.g., vehicle type, brand, etc.)
            var vehicles = _dbContext.Vehicles.AsQueryable();

            // Example of filtering (you can modify this based on the actual request properties)
            if (!string.IsNullOrEmpty(request.VehicleType))
            {
                vehicles = vehicles.Where(v => v.VehicleType == request.VehicleType);
            }

            if (!string.IsNullOrEmpty(request.VehicleBrand))
            {
                vehicles = vehicles.Where(v => v.VehicleBrand == request.VehicleBrand);
            }

            return vehicles.Select(v => new GetVehicleResponse
            {
                Id = v.Id,
                VehicleNumber = v.VehicleNumber,
                VehicleType = v.VehicleType,
                VehicleBrand = v.VehicleBrand
            }).ToList();
        }

        // Get vehicle by ID
        public GetVehicleResponse GetVehicleById(Guid vehicleId)
        {
            var vehicle = _dbContext.Vehicles.FirstOrDefault(v => v.Id == vehicleId);
            if (vehicle == null)
            {
                return null;
            }

            return new GetVehicleResponse
            {
                Id = vehicle.Id,
                VehicleNumber = vehicle.VehicleNumber,
                VehicleType = vehicle.VehicleType,
                VehicleBrand = vehicle.VehicleBrand,
                UserId = vehicle.UserId
            };
        }

        // Update a vehicle
        public async Task<BaseResponse> UpdateVehicle(UpdateVehicleRequest request)
        {
            var vehicle = await _dbContext.Vehicles.FirstOrDefaultAsync(v => v.Id == request.VehicleId);
            if (vehicle == null)
            {
                return new BaseResponse { IsSuccess = false, ErrorMessage = "Vehicle not found" };
            }

            // Update vehicle properties
            vehicle.VehicleNumber = request.VehicleNumber;
            vehicle.VehicleType = request.VehicleType;
            vehicle.VehicleBrand = request.VehicleBrand;

            _dbContext.Vehicles.Update(vehicle);
            await _dbContext.SaveChangesAsync();

            return new BaseResponse { IsSuccess = true };
        }

        // Create a new vehicle asynchronously
        public async Task CreateVehicleAsync(CreateVehicle request)
        {
            var newVehicle = new Vehicle
            {
                Id = Guid.NewGuid(),
                VehicleNumber = request.VehicleNumber,
                VehicleType = request.VehicleType,
                VehicleBrand = request.VehicleBrand
            };

            await _dbContext.Vehicles.AddAsync(newVehicle);
            await _dbContext.SaveChangesAsync();
        }

        // Delete a vehicle asynchronously
        public async Task DeleteVehicleAsync(Guid vehicleId)
        {
            var vehicle = await _dbContext.Vehicles.FirstOrDefaultAsync(v => v.Id == vehicleId);
            if (vehicle == null)
            {
                throw new KeyNotFoundException("Vehicle not found");
            }

            _dbContext.Vehicles.Remove(vehicle);
            await _dbContext.SaveChangesAsync();
        }
    }
}
