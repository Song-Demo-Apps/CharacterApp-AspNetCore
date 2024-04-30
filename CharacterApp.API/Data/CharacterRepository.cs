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
        Character? characterToDelete = await _context.Characters.FindAsync(id);
        if(characterToDelete is not null)
        {
            _context.Characters.Remove(characterToDelete);
            await _context.SaveChangesAsync();
        }
        return characterToDelete;
    }

    public async Task<Character?> GetCharacterById(int id)
    {
        return await _context.Characters.Include(c => c.CharacterItems).Include(c => c.CharacterSpeices).FirstOrDefaultAsync(c => c.Id == id);
    }
    public async Task<Character?> GetCharacterByName(string name, bool includeItems = false)
    {
        if(includeItems) {
            return await _context.Characters.Include(c => c.CharacterItems).Include(c => c.CharacterSpeices).FirstOrDefaultAsync(c => c.Name == name);

        }
        return await _context.Characters.Include(c => c.CharacterSpeices).FirstOrDefaultAsync(c => c.Name == name);
    }
    public async Task<List<Character>> GetCharactersBySpeices(int speicesId)
    {
        return await _context.Characters.Include(c => c.CharacterSpeices).Where(c => c.CharacterSpeices.Id == speicesId).ToListAsync();
    }

    public async Task<List<Character>> GetCharacters(int offset, int limit, string search)
    {
        return await _context.Characters.Include(c => c.CharacterSpeices).OrderBy(c => c.Id).Where(c => c.Id > offset && c.Contains(search)).Take(limit).ToListAsync();
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