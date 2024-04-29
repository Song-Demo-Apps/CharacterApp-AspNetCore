using CharacterApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CharacterApp.Data;

public class ItemRepository : IItemRepository
{
    private readonly CharacterDbContext _context;
    private readonly ILogger<ItemRepository> _logger;

    public ItemRepository(CharacterDbContext context, ILogger<ItemRepository> logger) => (_context, _logger) = (context, logger);

    /// <summary>
    /// Creates a new <see cref="Item"/> object in the database.
    /// </summary>
    /// <param name="item">The <see cref="Item"/> object to be created.</param>
    /// <returns>The created <see cref="Item"/> object.</returns>
    public async Task<Item> CreateItemAsync(Item item)
    {
        _logger.LogDebug($"Creating new {nameof(Item)} object in the database.");

        // Add the item object to the database
        _context.Items.Add(item);

        _logger.LogDebug($"Item object added to the database.");

        // Save the changes to the database
        await _context.SaveChangesAsync();

        _logger.LogDebug($"Changes saved to the database.");

        // Return the created item object
        _logger.LogDebug($"Returning created {nameof(Item)} object.");
        return item;
    }

    /// <summary>
    /// Deletes a <see cref="Item"/> object from the database.
    /// </summary>
    /// <param name="item">The <see cref="Item"/> object to be deleted.</param>
    /// <returns>The deleted <see cref="Item"/> object.</returns>
    public async Task<Item> DeleteItemAsync(Item item)
    {
        _logger.LogInformation($"Deleting {nameof(Item)} object with Id {item.Id} from the database.");

        // Remove the item object from the database
        _context.Items.Remove(item);

        _logger.LogInformation($"{nameof(Item)} object with Id {item.Id} removed from the database.");

        // Save the changes to the database
        await _context.SaveChangesAsync();

        _logger.LogInformation($"Changes saved to the database.");

        // Return the deleted item object
        _logger.LogInformation($"Returning deleted {nameof(Item)} object with Id {item.Id}.");
        return item;
    }

    /// <summary>
    /// Retrieves a list of all <see cref="Item"/> objects from the database, 
    /// starting from the specified <paramref name="offset"/>, with a maximum of <paramref name="limit"/> 
    /// number of objects. The objects are ordered by their Id in ascending order.
    /// </summary>
    /// <param name="offset">The Id of the first object to retrieve. Default is 0.</param>
    /// <param name="limit">The maximum number of objects to retrieve. Default is 100.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a list of <see cref="Item"/> objects.</returns>
    public async Task<List<Item>> GetAllItemAsync(int offset, int limit, string searchTerm = "")
    {
        _logger.LogDebug($"Retrieving all {nameof(Item)} objects that matches {searchTerm} from the database, starting from Id {offset}, with a maximum of {limit} objects.");
        // Retrieve all Item objects from the database, starting from the specified offset,
        // ordered by their Id in ascending order, and taking a maximum of limit number of objects.
        List<Item> result = await _context.Items
            .OrderBy(s => s.Id) // Order the Item objects by their Id in ascending order
            .Where(s => s.Id > offset && (s.Name.Contains(searchTerm) ||(s.Description != null && s.Description.Contains(searchTerm)))) // Filter out the objects with Id smaller than or equal to the offset and the search term
            .Take(limit) // Take only the specified number of objects
            .ToListAsync(); // Materialize the query results into a List

        _logger.LogDebug($"Finished retrieving {nameof(Item)} objects from the database.");

        return result;
    }

    /// <summary>
    /// Retrieves a <see cref="Item"/> object from the database by its Id.
    /// </summary>
    /// <param name="id">The Id of the <see cref="Item"/> object to retrieve.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the <see cref="Item"/> object
    /// with the specified Id, or null if no such object exists.</returns>
    public async Task<Item?> GetItemByIdAsync(int id)
    {
        _logger.LogDebug($"Retrieving {nameof(Item)} object with Id {id} from the database.");

        // Retrieve a Item object from the database by its Id using the FindAsync method.
        // The method returns a task representing the asynchronous operation. The task result
        // contains the Item object with the specified Id, or null if no such object exists.
        Item? item = await _context.Items.FindAsync(id);

        if (item == null)
        {
            _logger.LogDebug($"No {nameof(Item)} object with Id {id} found in the database.");
        }
        else
        {
            _logger.LogDebug($"{nameof(Item)} object with Id {id} retrieved from the database.");
        }

        return item;
    }


    /// <summary>
    /// Updates a <see cref="Item"/> object in the database.
    /// </summary>
    /// <param name="item">The <see cref="Item"/> object to be updated.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the updated <see cref="Item"/> object.</returns>
    public async Task<Item?> UpdateItemAsync(Item item)
    {
        _logger.LogDebug($"Updating {nameof(Item)} object with Id {item.Id} in the database.");

        _logger.LogDebug($"Attaching updated {nameof(Item)} object to the context.");
        Item? found = await _context.Items.FindAsync((int) item.Id!);

        if(found is not null) {
            found.Name = string.IsNullOrWhiteSpace(item.Name) ? found.Name : item.Name;
            found.Description = string.IsNullOrWhiteSpace(item.Description) ? found.Description : item.Description;
            found.Value = item.Value ?? found.Value;
            found.ImageUrl = string.IsNullOrWhiteSpace(item.ImageUrl) ? found.ImageUrl : item.ImageUrl;

            _logger.LogDebug($"Saving changes to the database.");
            await _context.SaveChangesAsync();

            _logger.LogDebug($"{nameof(Item)} object with Id {item.Id} updated in the database.");

            _logger.LogDebug($"Returning updated {nameof(Item)} object.");
            return found;
        }
        return null;

    }

}