using Domain.DTO.Request;
using Domain.DTO.Response;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Repository;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _dbContext;

        public VehicleService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, AppDbContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }

        public async Task CreateVehicleAsync(CreateVehicle vehicle)
        {
            // Assign a new Guid for the vehicle if it's not already set
            if (vehicle.Id == Guid.Empty)
            {
                vehicle.Id = Guid.NewGuid();
            }

            Vehicle createVehicle = new()
            {
                Id = vehicle.Id,
                UserId = vehicle.UserId,
                User = vehicle.User,
                VehicleBrand = vehicle.VehicleBrand,
                VehicleNumber = vehicle.VehicleNumber,
                VehicleType = vehicle.VehicleType
            };

            // Add the vehicle to the Vehicles DbSet
            await _dbContext.Vehicles.AddAsync(createVehicle);

            // Save the changes to the database
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteVehicleAsync(Guid vehicleId)
        {
            var vehicle = await _dbContext.Vehicles.FindAsync(vehicleId);
            if (vehicle != null)
            {
                _dbContext.Vehicles.Remove(vehicle);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<Vehicle>> GetAllVehiclesAsync()
        {
            return await _dbContext.Vehicles.ToListAsync();
        }

        public GetVehicleResponse GetVehicleById(Guid vehicleId)
        {
            var result = _unitOfWork.Repository<Vehicle>().GetByIdAsync(vehicleId);
            if (result == null) return null;

            return new GetVehicleResponse
            {
                DriverName = result.User != null ? result.User.FirstName + " " + result.User.LastName : "No Driver Assigned",
                UserId = result.UserId,
                VehicleBrand = result.VehicleBrand,
                VehicleNumber = result.VehicleNumber,
                VehicleType = result.VehicleType,
                Id = result.Id,
            };
        }

        public List<GetVehicleResponse> GetVehicles(GetVehicleRequest request)
        {
            var result = _unitOfWork.VehicleRepository.GetVehicles(request);

            return result.Select(x => new GetVehicleResponse
            {
                Id = x.Id,
                DriverName = x.User != null ? x.User.FirstName + " " + x.User.LastName : "No Driver Assigned",  // Handle null User
                UserId = x.UserId,
                VehicleBrand = x.VehicleBrand,
                VehicleNumber = x.VehicleNumber,
                VehicleType = x.VehicleType
            }).ToList();
        }

        public async Task<BaseResponse> UpdateVehicle(UpdateVehicleRequest request)
        {
            var result = new BaseResponse
            {
                IsSuccess = false
            };

            var currentVehicle = _unitOfWork.VehicleRepository.GetByIdAsync(request.VehicleId);
            if (currentVehicle is null)
            {
                result.ErrorMessage = "Vehicle does not exist";
                return result;
            }

            currentVehicle.VehicleBrand = request.VehicleBrand;
            currentVehicle.VehicleNumber = request.VehicleNumber;
            currentVehicle.VehicleType = request.VehicleType;
            currentVehicle.UserId = request.UserId;

            _unitOfWork.VehicleRepository.Update(currentVehicle);

            var dbResult = await _unitOfWork.SaveChanges() > 0;
            if (dbResult) 
            {
                result.IsSuccess = true;
            } else
            {
                result.ErrorMessage = "Failed when saving to database! Try Again later";
            }

            return result;
        }
    }
}
