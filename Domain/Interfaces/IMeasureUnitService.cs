using Domain.DTO.Request;
using Domain.DTO.Response;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IMeasureUnitService
    {
        Task<List<MeasureUnitResponse>> GetAllMesureUnitsAsync();
        Task<MeasureUnitResponse?> GetMesureUnitByIdAsync(int id);
        Task<MeasureUnitResponse> CreateMesureUnitAsync(MesureUnitRequest measureUnit);
        Task<MeasureUnitResponse> UpdateMesureUnitAsync(int id, MesureUnitRequest measureUnit);
        Task<bool> DeleteMesureUnitAsync(int id);
    }
}
