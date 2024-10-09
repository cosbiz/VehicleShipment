using Domain.DTO.Response;

namespace Domain.Interfaces
{
    public interface IUserService
    {
        Task<List<UserResponse>> GetUsersAsync();
        Task<UserResponse?> GetUserByIdAsync(string userId);
    }
}
