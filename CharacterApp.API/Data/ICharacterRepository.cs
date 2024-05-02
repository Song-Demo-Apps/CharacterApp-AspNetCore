using CharacterApp.Models;
using CharacterApp.Models.DTO;

namespace CharacterApp.Data;

public interface ICharacterRepository
{
    Task<List<CharacterOnlyDTO>> GetCharactersAsync(int offset, int limit, string search);
    Task<Character?> GetCharacterByIdAsync(int id);
    Task<Character> CreateCharacterAsync(Character newCharacter);
    Task<Character> UpdateCharacterAsync(Character characterToUpdate);
    Task<Character?> DeleteCharacterAsync(int id);

}