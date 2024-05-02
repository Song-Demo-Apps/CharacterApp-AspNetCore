using Microsoft.AspNetCore.Mvc;
using CharacterApp.Models;
using CharacterApp.Models.DTO;
using CharacterApp.Services;

namespace CharacterApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CharacterController : ControllerBase
{
    
    private readonly ICharacterService _characterService;
    private readonly ILogger<CharacterController> _logger;

    public CharacterController(ICharacterService characterService, ILogger<CharacterController> logger)
    {
        _characterService = characterService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<List<CharacterOnlyDTO>> GetCharacters(int offset = 0, int limit = 100, string search = "")
    {
        return await _characterService.GetCharactersAsync(offset, limit, search);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCharacterById(int id)
    {
        try
        {
            Character? character = await _characterService.GetCharacterByIdAsync(id);
            if(character is not null)
            {
                return Ok(character);
            }
            else
            {
                return NoContent();
            }
        }
        catch(ArgumentOutOfRangeException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<Character> CreateCharacter(CharacterOnlyDTO newCharacter)
    {
        return await _characterService.CreateCharacterAsync(newCharacter);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCharacter(CharacterOnlyDTO characterToUpdate)
    {
        try
        {
            Character? character = await _characterService.UpdateCharacterAsync(characterToUpdate);
            if(character is not null)
            {
                return Ok(character);
            }
            else
            {
                return NoContent();
            }
            
        }
        catch(ArgumentNullException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteCharacter(int id)
    {
        try
        {
            Character? character = await _characterService.DeleteCharacterAsync(id);
            if(character is not null)
            {
                return Ok(character);
            }
            else
            {
                return NoContent();
            }
        }
        catch(ArgumentOutOfRangeException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("inventory")]
    public async Task<IActionResult> PurchaseItems(OrderDTO order)
    {
        try
        {
            return Ok(await _characterService.PurchaseItemsAsync(order));
        }
        catch(KeyNotFoundException e)
        {
            return BadRequest(e.Message);
        }
        catch(ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }
}