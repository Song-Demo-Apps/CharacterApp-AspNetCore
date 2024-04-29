using CharacterApp.Models;

namespace CharacterApp.Data;

public interface IItemRepository 
{
    public Task<List<Item>> GetAllItemAsync(int offset, int limit, string searchTerm);
    public Task<Item?> GetItemByIdAsync(int id);
    public Task<Item?> UpdateItemAsync(Item speices);
    public Task<Item> CreateItemAsync(Item speices);
    public Task<Item> DeleteItemAsync(Item speices);
}