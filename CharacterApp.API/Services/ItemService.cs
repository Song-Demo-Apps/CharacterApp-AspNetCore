using CharacterApp.Data;
using CharacterApp.Models;

namespace CharacterApp.Services;

public class ItemService : IItemService
{
    private readonly IItemRepository _repo;
    private readonly ILogger<ItemService> _logger;

    public ItemService(IItemRepository repo, ILogger<ItemService> logger) => (_repo, _logger) = (repo, logger);

    /// <summary>
    /// Creates a new <see cref="Item"/> object in the database.
    /// </summary>
    /// <param name="speices">The <see cref="Item"/> object to be created. 
    /// The Id property must be null.</param>
    /// <exception cref="FormatException">Thrown when the provided speices object contains a non-null Id.</exception>
    /// <returns>A task representing the asynchronous operation. The task result contains the created <see cref="Item"/> object.</returns>
    public async Task<Item> CreateItemAsync(Item speices)
    {
        // Log the entry of the method
        _logger.LogDebug($"Entering {nameof(CreateItemAsync)} method of {nameof(ItemService)} class");

        // Check if the Id property of the provided speices object is null
        if(speices.Id is not null)
        {
            // Log the error and throw an exception if the Id property is not null
            _logger.LogError("New speices object cannot contain hardcoded id");
            throw new FormatException("New speices object cannot contain hardcoded id");
        }


        // Call the CreateItem method of the repository and return the result
        Item result = await _repo.CreateItemAsync(speices);

        // Log the creation of the speices object
        _logger.LogInformation($"Item object with Id {result.Id} created");

        // Return the created speices object
        return result;
    }

    /// <summary>
    /// Deletes a <see cref="Item"/> object from the database.
    /// </summary>
    /// <param name="id">The Id of the <see cref="Item"/> object to be deleted.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the deleted <see cref="Item"/> object, 
    /// or null if no such object exists.</returns>
    public async Task<Item> DeleteItemAsync(int id)
    {
        // Log the entry of the method
        _logger.LogDebug($"Entering {nameof(DeleteItemAsync)} method of {nameof(ItemService)} class");

        // Retrieve the speices object with the specified Id from the repository
        Item? found = await _repo.GetItemByIdAsync(id);

        // If the object exists, delete it and return the deleted object
        if(found is not null)
        {
            var result = await _repo.DeleteItemAsync(found);

            // Log the deletion of the speices object
            _logger.LogInformation($"Item object with Id {id} deleted");

            // Return the deleted speices object
            return result;
        }
        else
        {
            // Log the absence of the speices object
            _logger.LogDebug($"No {nameof(Item)} object with Id {id} found in the database");
            throw new KeyNotFoundException($"Speice with the id {id} was not found");
        }
    }

    /// <summary>
    /// Retrieves a list of all <see cref="Item"/> objects from the database, with an optional offset and limit.
    /// </summary>
    /// <param name="offset">The offset from which to retrieve the objects. Must be greater than or equal to 0.</param>
    /// <param name="limit">The maximum number of objects to retrieve. Must be greater than or equal to 1.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a list of <see cref="Item"/> objects.</returns>
    /// <exception cref="FormatException">Thrown if the offset is less than 0 or if the limit is less than 1.</exception>
    public async Task<List<Item>> GetAllItemAsync(int offset, int limit, string searchTerm)
    {
        // Check if the offset is less than 0
        if(offset < 0) {
            // Log the error and throw an exception if the offset is less than 0
            _logger.LogError("Offset must be greater than or equal to 0");
            throw new FormatException("Offset must be greater than or equal to 0");
        }

        // Check if the limit is less than 1
        if(limit < 1) {
            // Log the error and throw an exception if the limit is less than 1
            _logger.LogError("Limit must be greater than or equal to 1");
            throw new FormatException("Limit must be greater than or equal to 1");
        }
        // Log the retrieval of speices objects
        _logger.LogDebug($"Retrieving {limit} speices objects starting from offset {offset}");

        // Retrieve the speices objects from the repository
        List<Item> result = await _repo.GetAllItemAsync(offset, limit, searchTerm);

        // Log the successful retrieval of speices objects
        _logger.LogDebug($"{result.Count} speices objects retrieved");

        // Return the retrieved speices objects
        return result;
    }

    /// <summary>
    /// Retrieves a <see cref="Item"/> object from the database by its Id.
    /// </summary>
    /// <param name="id">The Id of the <see cref="Item"/> object to retrieve.</param>
    /// <returns>A task representing the asynchronous operation. The task result 
    /// contains the retrieved <see cref="Item"/> object, or null if no such object exists.</returns>
    public Task<Item?> GetItemByIdAsync(int id)
    {
        // Retrieves a Item object from the database by its Id.
        // Parameters:
        //   id: The Id of the Item object to retrieve.
        // Returns:
        //   A task representing the asynchronous operation. The task result 
        //   contains the retrieved Item object, or null if no such object exists.

        // Log the retrieval of speices object by Id
        _logger.LogDebug($"Retrieving speices object with Id {id}");

        // Call the GetItemById method of the repository and return the result
        return _repo.GetItemByIdAsync(id);
    }


    /// <summary>
    /// Updates a <see cref="Item"/> object in the database.
    /// </summary>
    /// <param name="speices">The <see cref="Item"/> object to be updated. 
    /// The Id property must not be null.</param>
    /// <exception cref="FormatException">Thrown if the speices object does not contain an Id property.</exception>
    /// <exception cref="KeyNotFoundException">Thrown if the speices object with the specified Id does not exist in the database.</exception>
    /// <returns>A task representing the asynchronous operation. The task result contains the updated <see cref="Item"/> object.</returns>
    public async Task<Item?> UpdateItemAsync(Item speices)
    {
        // Check if the Id property of the provided speices object is null
        if(speices.Id is null)
        {
            // Log the error and throw an exception if the speices object does not contain an Id property
            _logger.LogError("Item must contain Id property");
            throw new FormatException("Item must contain Id property");
        }

        // Retrieve the speices object with the specified Id from the repository
        Item? found = await _repo.GetItemByIdAsync((int) speices.Id);

        // If the object does not exist, log the error and throw a KeyNotFoundException
        if(found is null)
        {
            _logger.LogError($"Speice with the id {speices.Id} was not found");
            throw new KeyNotFoundException($"Speice with the id {speices.Id} was not found");
        }

        // Call the UpdateItem method of the repository and return the result
        return await _repo.UpdateItemAsync(speices);
    }
}