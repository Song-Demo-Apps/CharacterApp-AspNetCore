using CharacterApp.Models;
using CharacterApp.Models.DTO;

namespace CharacterApp.Data;

public interface ICharacterRepository
{
    Task<List<Character>> GetCharacters(int offset, int limit, string search);
    Task<Character?> GetCharacterById(int id);
    Task<Character> CreateCharacter(Character newCharacter);
    Task<Character> UpdateCharacter(Character characterToUpdate);
    Task<Character?> DeleteCharacter(int id);

}