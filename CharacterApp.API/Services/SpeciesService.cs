using CharacterApp.Data;
using CharacterApp.Models;

namespace CharacterApp.Services;

public class SpeciesService : ISpeciesService
{
    private readonly ISpeciesRepository _repo;
    private readonly ILogger<SpeciesService> _logger;

    public SpeciesService(ISpeciesRepository repo, ILogger<SpeciesService> logger) => (_repo, _logger) = (repo, logger);

    /// <summary>
    /// Creates a new <see cref="Species"/> object in the database.
    /// </summary>
    /// <param name="species">The <see cref="Species"/> object to be created. 
    /// The Id property must be null.</param>
    /// <exception cref="FormatException">Thrown when the provided species object contains a non-null Id.</exception>
    /// <returns>A task representing the asynchronous operation. The task result contains the created <see cref="Species"/> object.</returns>
    public async Task<Species> CreateSpeciesAsync(Species species)
    {
        // Log the entry of the method
        _logger.LogDebug($"Entering {nameof(CreateSpeciesAsync)} method of {nameof(SpeciesService)} class");

        // Check if the Id property of the provided species object is null
        if(species.Id is not null)
        {
            // Log the error and throw an exception if the Id property is not null
            _logger.LogError("New species object cannot contain hardcoded id");
            throw new FormatException("New species object cannot contain hardcoded id");
        }


        // Call the CreateSpecies method of the repository and return the result
        Species result = await _repo.CreateSpeciesAsync(species);

        // Log the creation of the species object
        _logger.LogInformation($"Species object with Id {result.Id} created");

        // Return the created species object
        return result;
    }

    /// <summary>
    /// Deletes a <see cref="Species"/> object from the database.
    /// </summary>
    /// <param name="id">The Id of the <see cref="Species"/> object to be deleted.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the deleted <see cref="Species"/> object, 
    /// or null if no such object exists.</returns>
    public async Task<Species?> DeleteSpeciesAsync(int id)
    {
        // Log the entry of the method
        _logger.LogDebug($"Entering {nameof(DeleteSpeciesAsync)} method of {nameof(SpeciesService)} class");

        // Retrieve the species object with the specified Id from the repository
        Species? found = await _repo.GetSpeciesByIdAsync(id);

        // If the object exists, delete it and return the deleted object
        if(found is not null)
        {
            var result = await _repo.DeleteSpeciesAsync(found);

            // Log the deletion of the species object
            _logger.LogInformation($"Species object with Id {id} deleted");

            // Return the deleted species object
            return result;
        }
        else
        {
            // Log the absence of the species object
            _logger.LogDebug($"No {nameof(Species)} object with Id {id} found in the database");
            throw new KeyNotFoundException($"Speice with the id {id} was not found");
        }
    }

    /// <summary>
    /// Retrieves a list of all <see cref="Species"/> objects from the database, with an optional offset and limit.
    /// </summary>
    /// <param name="offset">The offset from which to retrieve the objects. Must be greater than or equal to 0.</param>
    /// <param name="limit">The maximum number of objects to retrieve. Must be greater than or equal to 1.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a list of <see cref="Species"/> objects.</returns>
    /// <exception cref="FormatException">Thrown if the offset is less than 0 or if the limit is less than 1.</exception>
    public async Task<List<Species>> GetAllSpeciesAsync(int offset, int limit)
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
        // Log the retrieval of species objects
        _logger.LogDebug($"Retrieving {limit} species objects starting from offset {offset}");

        // Retrieve the species objects from the repository
        List<Species> result = await _repo.GetAllSpeciesAsync(offset, limit);

        // Log the successful retrieval of species objects
        _logger.LogDebug($"{result.Count} species objects retrieved");

        // Return the retrieved species objects
        return result;
    }

    /// <summary>
    /// Retrieves a <see cref="Species"/> object from the database by its Id.
    /// </summary>
    /// <param name="id">The Id of the <see cref="Species"/> object to retrieve.</param>
    /// <returns>A task representing the asynchronous operation. The task result 
    /// contains the retrieved <see cref="Species"/> object, or null if no such object exists.</returns>
    public Task<Species?> GetSpeciesByIdAsync(int id)
    {
        // Retrieves a Species object from the database by its Id.
        // Parameters:
        //   id: The Id of the Species object to retrieve.
        // Returns:
        //   A task representing the asynchronous operation. The task result 
        //   contains the retrieved Species object, or null if no such object exists.

        // Log the retrieval of species object by Id
        _logger.LogDebug($"Retrieving species object with Id {id}");

        // Call the GetSpeciesById method of the repository and return the result
        return _repo.GetSpeciesByIdAsync(id);
    }


    /// <summary>
    /// Updates a <see cref="Species"/> object in the database.
    /// </summary>
    /// <param name="species">The <see cref="Species"/> object to be updated. 
    /// The Id property must not be null.</param>
    /// <exception cref="FormatException">Thrown if the species object does not contain an Id property.</exception>
    /// <exception cref="KeyNotFoundException">Thrown if the species object with the specified Id does not exist in the database.</exception>
    /// <returns>A task representing the asynchronous operation. The task result contains the updated <see cref="Species"/> object.</returns>
    public async Task<Species?> UpdateSpeciesAsync(Species species)
    {
        // Check if the Id property of the provided species object is null
        if(species.Id is null)
        {
            // Log the error and throw an exception if the species object does not contain an Id property
            _logger.LogError("Species must contain Id property");
            throw new FormatException("Species must contain Id property");
        }

        // Retrieve the species object with the specified Id from the repository
        Species? found = await _repo.GetSpeciesByIdAsync((int) species.Id);

        // If the object does not exist, log the error and throw a KeyNotFoundException
        if(found is null)
        {
            _logger.LogError($"Speice with the id {species.Id} was not found");
            throw new KeyNotFoundException($"Speice with the id {species.Id} was not found");
        }

        // Call the UpdateSpecies method of the repository and return the result
        return await _repo.UpdateSpeciesAsync(species);
    }
}