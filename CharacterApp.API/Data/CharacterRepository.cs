using CharacterApp.Models;
using CharacterApp.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace CharacterApp.Data;

public class CharacterRepository : ICharacterRepository
{
    private readonly CharacterDbContext _context;
    private readonly ILogger<CharacterRepository> _logger;
    public CharacterRepository(CharacterDbContext context, ILogger<CharacterRepository> logger) => (_context, _logger) = (context, logger);

    public async Task<Character> CreateCharacterAsync(Character newCharacter)
    {
        _context.Characters.Add(newCharacter);
        await _context.SaveChangesAsync();

        return newCharacter;
    }

    public async Task<Character?> DeleteCharacterAsync(int id)
    {
        Character? characterToDelete = await _context.Characters.FindAsync(id);
        if(characterToDelete is not null)
        {
            _context.Characters.Remove(characterToDelete);
            await _context.SaveChangesAsync();
        }
        return characterToDelete;
    }

    public async Task<Character?> GetCharacterByIdAsync(int id)
    {
        return await _context.Characters
        .Include(c => c.CharacterItems)
        .ThenInclude(ci => ci.Item)
        .Include(c => c.CharacterSpecies)
        .FirstOrDefaultAsync(c => c.Id == id);
    }
    public async Task<Character?> GetCharacterByNameAsync(string name, bool includeItems = false)
    {
        if(includeItems) {
            return await _context.Characters
            .Include(c => c.CharacterItems)
            .ThenInclude(ci => ci.Item)
            .Include(c => c.CharacterSpecies)
            .FirstOrDefaultAsync(c => c.Name == name);

        }
        return await _context.Characters
        .Include(c => c.CharacterSpecies)
        .FirstOrDefaultAsync(c => c.Name == name);
    }
    public async Task<List<CharacterOnlyDTO>> GetCharactersBySpeciesAsync(int speciesId)
    {
        return await _context.Characters
        .Include(c => c.CharacterSpecies)
        .Where(c => c.CharacterSpecies.Id == speciesId)
        .Select(c => new CharacterOnlyDTO(c))
        .ToListAsync();
    }

    public async Task<List<CharacterOnlyDTO>> GetCharactersAsync(int offset, int limit, string search)
    {
        return await _context.Characters
        .Include(c => c.CharacterSpecies)
        .Where(c => c.Id > offset)
        .Where(c => (c.Name + c.Bio).Contains(search))
        .OrderBy(c => c.Id)
        .Take(limit)
        .Select(c => new CharacterOnlyDTO(c))
        .ToListAsync();
    }

    public async Task<Character> UpdateCharacterAsync(Character characterToUpdate)
    {   
        _context.Update(characterToUpdate);
        await _context.SaveChangesAsync();

        return characterToUpdate;
    }
}