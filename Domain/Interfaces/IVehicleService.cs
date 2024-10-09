using Domain.DTO.Request;
using Domain.DTO.Response;

namespace Domain.Interfaces
{
    public interface IVehicleService
    {
        List<GetVehicleResponse> GetVehicles(GetVehicleRequest request);
        GetVehicleResponse GetVehicleById(Guid vehicleId);
    }
}
