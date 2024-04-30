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
    public async Task<CharacterOnlyDTO> CreateCharacter(CharacterOnlyDTO newCharacter)
    {
        throw new NotImplementedException();
    }

    [HttpPut]
    public async Task<CharacterOnlyDTO> UpdateCharacter(CharacterOnlyDTO characterToUpdate)
    {
        throw new NotImplementedException();
    }

    [HttpDelete]
    public async Task<CharacterOnlyDTO> DeleteCharacter(int id)
    {
        throw new NotImplementedException();
    }

    [HttpPost("sale")]
    public Task<Character> PurchaseItems(OrderDTO items)
    {
        throw new NotImplementedException();
    }
}