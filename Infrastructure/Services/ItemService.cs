using Domain.DTO.Request;
using Domain.DTO.Response;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class ItemService : IItemService
    {
        private readonly AppDbContext _dbContext;

        public ItemService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Get item by code
        public async Task<ItemResponse?> GetItemByIdAsync(int code)
        {
            var item = await _dbContext.Items
                .Include(i => i.User)
                .Include(i => i.MesureUnit)
                .FirstOrDefaultAsync(i => i.Code == code);

            if (item == null) return null;

            return new ItemResponse
            {
                Code = item.Code,
                Name = item.Name,
                Quantity = item.Quantity,
                Description = item.Description,
                RefereceNumber = item.RefereceNumber,
                MeasureUnit = item.MesureUnit.Name,
                User = $"{item.User.FirstName} {item.User.LastName}"
            };
        }

        // Create a new item
        public async Task<ItemResponse> CreateItemAsync(ItemRequest itemRequest)
        {
            // Ensure that the User exists
            var user = await _dbContext.Users.FindAsync(itemRequest.UserId);
            if (user == null) throw new KeyNotFoundException("User not found.");

            // Ensure that the MeasureUnit exists
            var measureUnit = await _dbContext.MesureUnits.FindAsync(itemRequest.MesureUnitId);
            if (measureUnit == null) throw new KeyNotFoundException("Measure unit not found.");

            var newItem = new ItemData
            {
                Code = await GetNextItemCode(),
                Name = itemRequest.Name,
                Quantity = itemRequest.Quantity,
                Description = itemRequest.Description,
                RefereceNumber = itemRequest.RefereceNumber,
                MesureUnitId = measureUnit.Id,
                UserId = user.Id
            };

            _dbContext.Items.Add(newItem);
            await _dbContext.SaveChangesAsync();

            return new ItemResponse
            {
                Code = newItem.Code,
                Name = newItem.Name,
                Quantity = newItem.Quantity,
                Description = newItem.Description,
                RefereceNumber = newItem.RefereceNumber,
                MeasureUnit = measureUnit.Name,
                User = $"{user.FirstName} {user.LastName}"
            };
        }

        // Update an existing item
        public async Task<ItemResponse> UpdateItemAsync(int code, ItemRequest itemRequest)
        {
            var existingItem = await _dbContext.Items.FindAsync(code);
            if (existingItem == null) throw new KeyNotFoundException("Item not found.");

            // Ensure that the User exists
            var user = await _dbContext.Users.FindAsync(itemRequest.UserId);
            if (user == null) throw new KeyNotFoundException("User not found.");

            // Ensure that the MeasureUnit exists
            var measureUnit = await _dbContext.MesureUnits.FindAsync(itemRequest.MesureUnitId);
            if (measureUnit == null) throw new KeyNotFoundException("Measure unit not found.");

            // Update item properties
            existingItem.Name = itemRequest.Name;
            existingItem.Quantity = itemRequest.Quantity;
            existingItem.Description = itemRequest.Description;
            existingItem.RefereceNumber = itemRequest.RefereceNumber;
            existingItem.MesureUnitId = measureUnit.Id;
            existingItem.UserId = user.Id;

            _dbContext.Items.Update(existingItem);
            await _dbContext.SaveChangesAsync();

            return new ItemResponse
            {
                Code = existingItem.Code,
                Name = existingItem.Name,
                Quantity = existingItem.Quantity,
                Description = existingItem.Description,
                RefereceNumber = existingItem.RefereceNumber,
                MeasureUnit = measureUnit.Name,
                User = $"{user.FirstName} {user.LastName}"
            };
        }

        // Delete an item by code
        public async Task<bool> DeleteItemAsync(int code)
        {
            var existingItem = await _dbContext.Items.FindAsync(code);
            if (existingItem == null) throw new KeyNotFoundException("Item not found.");

            _dbContext.Items.Remove(existingItem);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        // Method to get the next available item code
        private async Task<int> GetNextItemCode()
        {
            var lastItem = await _dbContext.Items.OrderByDescending(i => i.Code).FirstOrDefaultAsync();
            return lastItem == null ? 10000 : lastItem.Code + 1;
        }
    }
}
