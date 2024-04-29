using CharacterApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CharacterApp.Data;

public class SpeicesRepository : ISpeicesRepository
{
    private readonly CharacterDbContext _context;
    private readonly ILogger _logger;

    public SpeicesRepository(CharacterDbContext context, ILogger logger) => (_context, _logger) = (context, logger);

    /// <summary>
    /// Creates a new <see cref="Speices"/> object in the database.
    /// </summary>
    /// <param name="speices">The <see cref="Speices"/> object to be created.</param>
    /// <returns>The created <see cref="Speices"/> object.</returns>
    public async Task<Speices> CreateSpeicesAsync(Speices speices)
    {
        _logger.LogDebug($"Creating new {nameof(Speices)} object in the database.");

        // Add the speices object to the database
        _context.Speices.Add(speices);

        _logger.LogDebug($"Speices object added to the database.");

        // Save the changes to the database
        await _context.SaveChangesAsync();

        _logger.LogDebug($"Changes saved to the database.");

        // Return the created speices object
        _logger.LogDebug($"Returning created {nameof(Speices)} object.");
        return speices;
    }

    /// <summary>
    /// Deletes a <see cref="Speices"/> object from the database.
    /// </summary>
    /// <param name="speices">The <see cref="Speices"/> object to be deleted.</param>
    /// <returns>The deleted <see cref="Speices"/> object.</returns>
    public async Task<Speices> DeleteSpeicesAsync(Speices speices)
    {
        _logger.LogInformation($"Deleting {nameof(Speices)} object with Id {speices.Id} from the database.");

        // Remove the speices object from the database
        _context.Speices.Remove(speices);

        _logger.LogInformation($"{nameof(Speices)} object with Id {speices.Id} removed from the database.");

        // Save the changes to the database
        await _context.SaveChangesAsync();

        _logger.LogInformation($"Changes saved to the database.");

        // Return the deleted speices object
        _logger.LogInformation($"Returning deleted {nameof(Speices)} object with Id {speices.Id}.");
        return speices;
    }

    /// <summary>
    /// Retrieves a list of all <see cref="Speices"/> objects from the database, 
    /// starting from the specified <paramref name="offset"/>, with a maximum of <paramref name="limit"/> 
    /// number of objects. The objects are ordered by their Id in ascending order.
    /// </summary>
    /// <param name="offset">The Id of the first object to retrieve. Default is 0.</param>
    /// <param name="limit">The maximum number of objects to retrieve. Default is 100.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a list of <see cref="Speices"/> objects.</returns>
    public async Task<List<Speices>> GetAllSpeicesAsync(int offset = 0, int limit = 100)
    {
        _logger.LogDebug($"Retrieving all {nameof(Speices)} objects from the database, starting from Id {offset}, with a maximum of {limit} objects.");
        // Retrieve all Speices objects from the database, starting from the specified offset,
        // ordered by their Id in ascending order, and taking a maximum of limit number of objects.
        List<Speices> result = await _context.Speices
            .OrderBy(s => s.Id) // Order the Speices objects by their Id in ascending order
            .Where(s => s.Id > offset) // Filter out the objects with Id smaller than or equal to the offset
            .Take(limit) // Take only the specified number of objects
            .ToListAsync(); // Materialize the query results into a List

        _logger.LogDebug($"Finished retrieving {nameof(Speices)} objects from the database.");

        return result;
    }

    /// <summary>
    /// Retrieves a <see cref="Speices"/> object from the database by its Id.
    /// </summary>
    /// <param name="id">The Id of the <see cref="Speices"/> object to retrieve.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the <see cref="Speices"/> object
    /// with the specified Id, or null if no such object exists.</returns>
    public async Task<Speices?> GetSpeicesByIdAsync(int id)
    {
        _logger.LogDebug($"Retrieving {nameof(Speices)} object with Id {id} from the database.");

        // Retrieve a Speices object from the database by its Id using the FindAsync method.
        // The method returns a task representing the asynchronous operation. The task result
        // contains the Speices object with the specified Id, or null if no such object exists.
        Speices? speices = await _context.Speices.FindAsync(id);

        if (speices == null)
        {
            _logger.LogDebug($"No {nameof(Speices)} object with Id {id} found in the database.");
        }
        else
        {
            _logger.LogDebug($"{nameof(Speices)} object with Id {id} retrieved from the database.");
        }

        return speices;
    }


    /// <summary>
    /// Updates a <see cref="Speices"/> object in the database.
    /// </summary>
    /// <param name="speices">The <see cref="Speices"/> object to be updated.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the updated <see cref="Speices"/> object.</returns>
    public async Task<Speices> UpdateSpeicesAsync(Speices speices)
    {
        _logger.LogDebug($"Updating {nameof(Speices)} object with Id {speices.Id} in the database.");

        _logger.LogDebug($"Attaching updated {nameof(Speices)} object to the context.");
        _context.Update(speices);

        _logger.LogDebug($"Saving changes to the database.");
        await _context.SaveChangesAsync();

        _logger.LogDebug($"{nameof(Speices)} object with Id {speices.Id} updated in the database.");

        _logger.LogDebug($"Returning updated {nameof(Speices)} object.");
        return speices;
    }

}