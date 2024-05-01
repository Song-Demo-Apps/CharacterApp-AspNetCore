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
        throw new NotImplementedException();
    }

    [HttpGet("{id}")]
    public async Task<Character> GetCharacterById(int id)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public async Task<Character> CreateCharacter(CharacterOnlyDTO newCharacter)
    {
        throw new NotImplementedException();
    }

    [HttpPut]
    public async Task<Character> UpdateCharacter(CharacterOnlyDTO characterToUpdate)
    {
        throw new NotImplementedException();
    }

    [HttpDelete]
    public async Task<Character> DeleteCharacter(int id)
    {
        throw new NotImplementedException();
    }

    [HttpPost("inventory")]
    public Task<Character> PurchaseItems(OrderDTO items)
    {
        throw new NotImplementedException();
    }
}