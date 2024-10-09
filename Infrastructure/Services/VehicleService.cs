using Domain.DTO.Request;
using Domain.DTO.Response;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Repository;

namespace Infrastructure.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VehicleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
    }
}
