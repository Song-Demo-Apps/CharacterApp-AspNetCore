using CharacterApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CharacterApp.Data;

public class SpeciesRepository : ISpeciesRepository
{
    private readonly CharacterDbContext _context;
    private readonly ILogger<SpeciesRepository> _logger;

    public SpeciesRepository(CharacterDbContext context, ILogger<SpeciesRepository> logger) => (_context, _logger) = (context, logger);

    /// <summary>
    /// Creates a new <see cref="Species"/> object in the database.
    /// </summary>
    /// <param name="species">The <see cref="Species"/> object to be created.</param>
    /// <returns>The created <see cref="Species"/> object.</returns>
    public async Task<Species> CreateSpeciesAsync(Species species)
    {
        _logger.LogDebug($"Creating new {nameof(Species)} object in the database.");

        // Add the species object to the database
        _context.Species.Add(species);

        _logger.LogDebug($"Species object added to the database.");

        // Save the changes to the database
        await _context.SaveChangesAsync();

        _logger.LogDebug($"Changes saved to the database.");

        // Return the created species object
        _logger.LogDebug($"Returning created {nameof(Species)} object.");
        return species;
    }

    /// <summary>
    /// Deletes a <see cref="Species"/> object from the database.
    /// </summary>
    /// <param name="species">The <see cref="Species"/> object to be deleted.</param>
    /// <returns>The deleted <see cref="Species"/> object.</returns>
    public async Task<Species> DeleteSpeciesAsync(Species species)
    {
        _logger.LogInformation($"Deleting {nameof(Species)} object with Id {species.Id} from the database.");

        // Remove the species object from the database
        _context.Species.Remove(species);

        _logger.LogInformation($"{nameof(Species)} object with Id {species.Id} removed from the database.");

        // Save the changes to the database
        await _context.SaveChangesAsync();

        _logger.LogInformation($"Changes saved to the database.");

        // Return the deleted species object
        _logger.LogInformation($"Returning deleted {nameof(Species)} object with Id {species.Id}.");
        return species;
    }

    /// <summary>
    /// Retrieves a list of all <see cref="Species"/> objects from the database, 
    /// starting from the specified <paramref name="offset"/>, with a maximum of <paramref name="limit"/> 
    /// number of objects. The objects are ordered by their Id in ascending order.
    /// </summary>
    /// <param name="offset">The Id of the first object to retrieve. Default is 0.</param>
    /// <param name="limit">The maximum number of objects to retrieve. Default is 100.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a list of <see cref="Species"/> objects.</returns>
    public async Task<List<Species>> GetAllSpeciesAsync(int offset, int limit)
    {
        _logger.LogDebug($"Retrieving all {nameof(Species)} objects from the database, starting from Id {offset}, with a maximum of {limit} objects.");
        // Retrieve all Species objects from the database, starting from the specified offset,
        // ordered by their Id in ascending order, and taking a maximum of limit number of objects.
        List<Species> result = await _context.Species
            .OrderBy(s => s.Id) // Order the Species objects by their Id in ascending order
            .Where(s => s.Id > offset) // Filter out the objects with Id smaller than or equal to the offset
            .Take(limit) // Take only the specified number of objects
            .ToListAsync(); // Materialize the query results into a List

        _logger.LogDebug($"Finished retrieving {nameof(Species)} objects from the database.");

        return result;
    }

    /// <summary>
    /// Retrieves a <see cref="Species"/> object from the database by its Id.
    /// </summary>
    /// <param name="id">The Id of the <see cref="Species"/> object to retrieve.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the <see cref="Species"/> object
    /// with the specified Id, or null if no such object exists.</returns>
    public async Task<Species?> GetSpeciesByIdAsync(int id)
    {
        _logger.LogDebug($"Retrieving {nameof(Species)} object with Id {id} from the database.");

        // Retrieve a Species object from the database by its Id using the FindAsync method.
        // The method returns a task representing the asynchronous operation. The task result
        // contains the Species object with the specified Id, or null if no such object exists.
        Species? species = await _context.Species.FindAsync(id);

        if (species == null)
        {
            _logger.LogDebug($"No {nameof(Species)} object with Id {id} found in the database.");
        }
        else
        {
            _logger.LogDebug($"{nameof(Species)} object with Id {id} retrieved from the database.");
        }

        return species;
    }


    /// <summary>
    /// Updates a <see cref="Species"/> object in the database.
    /// </summary>
    /// <param name="species">The <see cref="Species"/> object to be updated.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the updated <see cref="Species"/> object.</returns>
    public async Task<Species?> UpdateSpeciesAsync(Species species)
    {
        _logger.LogDebug($"Updating {nameof(Species)} object with Id {species.Id} in the database.");

        _logger.LogDebug($"Attaching updated {nameof(Species)} object to the context.");
        Species? found = await _context.Species.FindAsync((int) species.Id!);

        if(found is not null) {
            found.Name = string.IsNullOrWhiteSpace(species.Name) ? found.Name : species.Name;
            found.Description = string.IsNullOrWhiteSpace(species.Description) ? found.Description : species.Description;
            _logger.LogDebug($"Saving changes to the database.");
            await _context.SaveChangesAsync();

            _logger.LogDebug($"{nameof(Species)} object with Id {species.Id} updated in the database.");

            _logger.LogDebug($"Returning updated {nameof(Species)} object.");
            return found;
        }
        return null;

    }

}