using CharacterApp.Models;

namespace CharacterApp.Services;

public interface IItemService 
{
    public Task<List<Item>> GetAllItemAsync(int offset, int limit, string searchTerm);
    public Task<Item?> GetItemByIdAsync(int id);
    public Task<Item?> UpdateItemAsync(Item item);
    public Task<Item> CreateItemAsync(Item item);
    public Task<Item> DeleteItemAsync(int id);
}