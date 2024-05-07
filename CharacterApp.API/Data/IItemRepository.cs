using CharacterApp.Models;

namespace CharacterApp.Data;

public interface IItemRepository 
{
    public Task<List<Item>> GetAllItemAsync(int offset, int limit, string searchTerm);
    public Task<Item?> GetItemByIdAsync(int id);
    public Task<Item?> UpdateItemAsync(Item species);
    public Task<Item> CreateItemAsync(Item species);
    public Task<Item> DeleteItemAsync(Item species);
}