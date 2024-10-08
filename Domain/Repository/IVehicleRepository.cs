using Domain.DTO.Request;
using Domain.Entities;

namespace Domain.Repository
{
    public interface IVehicleRepository : IGenericRepository<Vehicle>
    {
        List<Vehicle> GetVehicles(GetVehicleRequest request);
    }
}
