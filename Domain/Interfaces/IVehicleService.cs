using Domain.DTO.Request;
using Domain.DTO.Response;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IVehicleService
    {
        List<GetVehicleResponse> GetVehicles(GetVehicleRequest request);
        Task<List<Vehicle>> GetAllVehiclesAsync(); // Fetch all vehicles
        GetVehicleResponse GetVehicleById(Guid vehicleId);
        Task<BaseResponse> UpdateVehicle(UpdateVehicleRequest request);
        Task CreateVehicleAsync(CreateVehicle vehicle);
        Task DeleteVehicleAsync(Guid vehicleId);
    }
}
