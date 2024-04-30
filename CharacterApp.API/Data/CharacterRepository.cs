using CharacterApp.Models;
using CharacterApp.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace CharacterApp.Data;

public class CharacterRepository : ICharacterRepository
{
    private readonly CharacterDbContext _context;
    public CharacterRepository(CharacterDbContext context) => _context = context;

    public async Task<Character> CreateCharacter(CharacterOnlyDTO newCharacter)
    {
        Character character = new Character(newCharacter);
        _context.Characters.Add(character);
        await _context.SaveChangesAsync();

        return character;
    }

    public async Task<Character?> DeleteCharacter(int id)
    {
        Character? characterToDelete = _context.Characters.Find(id);
        if(characterToDelete is not null)
        {
            _context.Characters.Remove(characterToDelete);
            await _context.SaveChangesAsync();
        }
        return characterToDelete;
    }

    public Task<Character> GetCharacterById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Character>> GetCharacters(int offset, int limit, string search)
    {
        throw new NotImplementedException();
    }

    public Task<Character> UpdateCharacter(CharacterOnlyDTO characterToUpdate)
    {
        throw new NotImplementedException();
    }

    public Task<Character> UpdateCharacterInventory(OrderDTO items)
    {
        throw new NotImplementedException();
    }
}