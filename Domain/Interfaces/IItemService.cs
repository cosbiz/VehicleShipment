using Domain.DTO.Request;
using Domain.DTO.Response;

namespace Domain.Interfaces
{
    public interface IItemService
    {
        Task<ItemResponse?> GetItemByIdAsync(int code);
        Task<ItemResponse> CreateItemAsync(ItemRequest itemRequest);
        Task<ItemResponse> UpdateItemAsync(int code, ItemRequest itemRequest);
        Task<bool> DeleteItemAsync(int code);
    }
}
