using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CharacterApp.Models;
using Microsoft.EntityFrameworkCore;
using CharacterApp.Services;


namespace CharacterApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ItemController : ControllerBase
{
    
    private readonly IItemService _itemService;
    private readonly ILogger<ItemController> _logger;

    public ItemController(IItemService itemService, ILogger<ItemController> logger)
    {
        _itemService = itemService;
        _logger = logger;
    }

    //This method contains an example of query parameters. The argument offset and limit are being passed in as query params
    /// <summary>
    /// Retrieves a collection of all Item objects.
    /// </summary>
    /// <param name="offset">The number of objects to skip.</param>
    /// <param name="limit">The maximum number of objects to return.</param>
    /// <returns>A collection of Item objects.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Item>>> GetItem(int offset = 0, int limit = 100, string search = "")
    {
        // Call the GetAllItem method of the IItemService interface to retrieve a collection of all Item objects.
        // The GetAllItem method is responsible for retrieving a paginated collection of Item objects from the database.
        // The offset parameter specifies the number of objects to skip, and the limit parameter specifies the maximum number of objects to return.
        // The method returns a Task that represents the asynchronous operation.
        // The task result contains a collection of Item objects.
        try 
        {
            return await _itemService.GetAllItemAsync(offset, limit, search);
        }
        catch(FormatException e)
        {
            return BadRequest(e.Message);
        }
    }

    // GET: api/Item/5
    // This method utilizes a route parameter
    /// <summary>
    /// Retrieves a Item object by its Id.
    /// </summary>
    /// <param name="id">The Id of the Item object.</param>
    /// <returns>A Item object if found, or NoContent if not found.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Item>> GetItemById(int id)
    {
        // Call the GetItemById method of the IItemService interface to retrieve a Item object by its Id.
        // The GetItemById method is responsible for retrieving a Item object from the database by its Id.
        // The method returns a Task that represents the asynchronous operation.
        // The task result contains a Item object if found, or NoContent if not found.
        Item? item = await _itemService.GetItemByIdAsync(id);

        // If the species is null, return NoContent, indicating that the species with the given Id was not found.
        if (item is null)
        {
            return NoContent();
        }

        // Return the species object.
        return item;
    }

    // POST: api/Item

    /// <summary>
    /// Creates a new Item object.
    /// </summary>
    /// <param name="item">The Item object to be created.</param>
    /// <returns>The created Item object.</returns>
    /// <response code="201">The Item object was created successfully.</response>
    /// <response code="400">The request was malformed.</response>
    [HttpPost]
    public async Task<ActionResult<Item>> PostItem(Item item)
    {
        // Call the CreateItemAsync method of the IItemService interface to create a new Item object.
        // The CreateItemAsync method is responsible for creating a new Item object in the database.
        // The method returns a Task that represents the asynchronous operation.
        // The task result contains the created Item object.
        try
        {
            await _itemService.CreateItemAsync(item);

            // Return a 201 Created response with a Location header containing a link to the created species.
            return CreatedAtAction(nameof(GetItemById), new { id = item.Id }, item);
        }
        catch(FormatException e)
        {
            // Return a 400 Bad Request response with the exception message if the request was malformed.
            return BadRequest(e.Message);
        }
    }

    // PUT: api/Item/5
    // Updates a Item object in the database.
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
    // 
    // Parameters:
    //   item: The Item object to be updated.
    //
    // Returns:
    //   The updated Item object if successful, or NoContent if the species with the given Id was not found.
    //   On error, returns a BadRequest response with the exception message.
    [HttpPut]
    public async Task<ActionResult<Item>> PutItem(Item item)
    {
        try
        {
            // Call the UpdateItemAsync method of the IItemService interface to update the Item object.
            // The UpdateItemAsync method is responsible for updating a Item object in the database.
            // The method returns a Task that represents the asynchronous operation.
            // The task result contains the updated Item object, or null if the species with the given Id was not found.
            Item? result = await _itemService.UpdateItemAsync(item);

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
    /// Deletes a Item object from the database.
    /// </summary>
    /// <param name="id">The Id of the Item object to delete.</param>
    /// <returns>
    /// The deleted Item object if found, or NoContent if the species with the given Id was not found.
    /// On error, returns a BadRequest response with the exception message.
    /// </returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult<Item>> DeleteItem(int id)
    {
        try 
        {
            // Call the DeleteItemAsync method of the IItemService interface to delete the Item object.
            // The DeleteItemAsync method is responsible for deleting a Item object from the database.
            // The method returns a Task that represents the asynchronous operation.
            // The task result contains the deleted Item object, or null if the species with the given Id was not found.
            Item? result = await _itemService.DeleteItemAsync(id);

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