using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CharacterApp.Models;
using Microsoft.EntityFrameworkCore;
using CharacterApp.Services;


namespace CharacterApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SpeicesController : ControllerBase
{
    
    private readonly ISpeicesService _speicesService;
    private readonly ILogger<SpeicesController> _logger;

    public SpeicesController(ISpeicesService speicesService, ILogger<SpeicesController> logger)
    {
        _speicesService = speicesService;
        _logger = logger;
    }

    //This method contains an example of query parameters. The argument offset and limit are being passed in as query params
    /// <summary>
    /// Retrieves a collection of all Species objects.
    /// </summary>
    /// <param name="offset">The number of objects to skip.</param>
    /// <param name="limit">The maximum number of objects to return.</param>
    /// <returns>A collection of Species objects.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Speices>>> GetSpeices(int offset = 0, int limit = 100)
    {
        // Call the GetAllSpeices method of the ISpeicesService interface to retrieve a collection of all Species objects.
        // The GetAllSpeices method is responsible for retrieving a paginated collection of Species objects from the database.
        // The offset parameter specifies the number of objects to skip, and the limit parameter specifies the maximum number of objects to return.
        // The method returns a Task that represents the asynchronous operation.
        // The task result contains a collection of Species objects.
        try 
        {
            return await _speicesService.GetAllSpeicesAsync(offset, limit);
        }
        catch(FormatException e)
        {
            return BadRequest(e.Message);
        }
    }

    // GET: api/Speices/5
    // This method utilizes a route parameter
    /// <summary>
    /// Retrieves a Species object by its Id.
    /// </summary>
    /// <param name="id">The Id of the Species object.</param>
    /// <returns>A Species object if found, or NoContent if not found.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Speices>> GetSpeicesById(int id)
    {
        // Call the GetSpeicesById method of the ISpeicesService interface to retrieve a Species object by its Id.
        // The GetSpeicesById method is responsible for retrieving a Species object from the database by its Id.
        // The method returns a Task that represents the asynchronous operation.
        // The task result contains a Species object if found, or NoContent if not found.
        Speices? speices = await _speicesService.GetSpeicesByIdAsync(id);

        // If the species is null, return NoContent, indicating that the species with the given Id was not found.
        if (speices is null)
        {
            return NoContent();
        }

        // Return the species object.
        return speices;
    }

    // POST: api/Speices

    /// <summary>
    /// Creates a new Species object.
    /// </summary>
    /// <param name="speices">The Species object to be created.</param>
    /// <returns>The created Species object.</returns>
    /// <response code="201">The Species object was created successfully.</response>
    /// <response code="400">The request was malformed.</response>
    [HttpPost]
    public async Task<ActionResult<Speices>> PostSpeices(Speices speices)
    {
        // Call the CreateSpeicesAsync method of the ISpeicesService interface to create a new Species object.
        // The CreateSpeicesAsync method is responsible for creating a new Species object in the database.
        // The method returns a Task that represents the asynchronous operation.
        // The task result contains the created Species object.
        try
        {
            await _speicesService.CreateSpeicesAsync(speices);

            // Return a 201 Created response with a Location header containing a link to the created species.
            return CreatedAtAction(nameof(GetSpeicesById), new { id = speices.Id }, speices);
        }
        catch(FormatException e)
        {
            // Return a 400 Bad Request response with the exception message if the request was malformed.
            return BadRequest(e.Message);
        }
    }

    // PUT: api/Speices/5
    // Updates a Species object in the database.
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
    // 
    // Parameters:
    //   speices: The Species object to be updated.
    //
    // Returns:
    //   The updated Species object if successful, or NoContent if the species with the given Id was not found.
    //   On error, returns a BadRequest response with the exception message.
    [HttpPut]
    public async Task<ActionResult<Speices>> PutSpeices(Speices speices)
    {
        try
        {
            // Call the UpdateSpeicesAsync method of the ISpeicesService interface to update the Species object.
            // The UpdateSpeicesAsync method is responsible for updating a Species object in the database.
            // The method returns a Task that represents the asynchronous operation.
            // The task result contains the updated Species object, or null if the species with the given Id was not found.
            Speices? result = await _speicesService.UpdateSpeicesAsync(speices);

            if(result is null) 
            {
                // If the species is null, return NoContent, indicating that the species with the given Id was not found.
                return NoContent();
            }

            // Return the updated species object.
            return result;
        }
        catch(Exception e) when(e is FormatException || e is KeyNotFoundException)
        {
            // On error, return a BadRequest response with the exception message.
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Deletes a Species object from the database.
    /// </summary>
    /// <param name="id">The Id of the Species object to delete.</param>
    /// <returns>
    /// The deleted Species object if found, or NoContent if the species with the given Id was not found.
    /// On error, returns a BadRequest response with the exception message.
    /// </returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult<Speices>> DeleteSpeices(int id)
    {
        try 
        {
            // Call the DeleteSpeicesAsync method of the ISpeicesService interface to delete the Species object.
            // The DeleteSpeicesAsync method is responsible for deleting a Species object from the database.
            // The method returns a Task that represents the asynchronous operation.
            // The task result contains the deleted Species object, or null if the species with the given Id was not found.
            Speices? result = await _speicesService.DeleteSpeicesAsync(id);

            if(result is null)
            {
                // If the species is null, return NoContent, indicating that the species with the given Id was not found.
                return NoContent();
            }

            // Return the deleted species object.
            return Ok(result);
        }
        catch(KeyNotFoundException e)
        {
            // On error, return a BadRequest response with the exception message.
            return BadRequest(e.Message);
        }
    }
}