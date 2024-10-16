using Domain.DTO.Response;

namespace Domain.Interfaces
{
    public interface IUserService
    {
        Task<List<UserResponse>> GetUsersAsync();
        Task<UserResponse?> GetUserByIdAsync(string userId);
        Task<BaseResponse> UpdateUserAsync(UserResponse user);
        Task<List<UserResponse>> GetUsersWithVehiclesAsync();
        Task<UserResponse?> GetCurrentUserAsync();
        Task<UserResponse?> GetCurrentUserAppAsync();
    }
}
