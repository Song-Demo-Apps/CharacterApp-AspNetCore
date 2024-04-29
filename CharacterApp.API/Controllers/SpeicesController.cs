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

    public SpeicesController(ISpeicesService speicesService)
    {
        _speicesService = speicesService;
    }

    //This method contains an example of query parameters. The argument offset and limit are being passed in as query params
    /// <summary>
    /// Retrieves a collection of all Species objects.
    /// </summary>
    /// <param name="offset">The number of objects to skip.</param>
    /// <param name="limit">The maximum number of objects to return.</param>
    /// <returns>A collection of Species objects.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Speices>>> GetSpeices(int offset, int limit)
    {
        // Call the GetAllSpeices method of the ISpeicesService interface to retrieve a collection of all Species objects.
        // The GetAllSpeices method is responsible for retrieving a paginated collection of Species objects from the database.
        // The offset parameter specifies the number of objects to skip, and the limit parameter specifies the maximum number of objects to return.
        // The method returns a Task that represents the asynchronous operation.
        // The task result contains a collection of Species objects.
        return await _speicesService.GetAllSpeicesAsync(offset, limit);
    }

    // GET: api/Speices/5
    // This method utilizes a route parameter
    [HttpGet("{id}")]
    public async Task<ActionResult<Speices>> GetSpeicesById(int id)
    {
        Speices? speices = await _speicesService.GetSpeicesByIdAsync(id);

        if (speices is null)
        {
            return NoContent();
        }

        return speices;
    }

    // POST: api/Speices
    // To protect from overposting attacks, enable the specific properties you want to bind to, for
    // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
    [HttpPost]
    public async Task<ActionResult<Speices>> PostSpeices(Speices speices)
    {
        await _speicesService.CreateSpeicesAsync(speices);
        
        return CreatedAtAction("GetSpeicesById", new { id = speices.Id }, speices);
    }

    // PUT: api/Speices/5
    // To protect from overposting attacks, enable the specific properties you want to bind to, for
    // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
    [HttpPut]
    public async Task<Speices> PutSpeices(Speices speices)
    {
        return await _speicesService.UpdateSpeicesAsync(speices);
    }

    // DELETE: api/Speices/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSpeices(int id)
    {
        Speices? result = await _speicesService.DeleteSpeicesAsync(id);
        
        if(result is null)
        {
            return NoContent();
        }

        return Ok(result);
    }

}