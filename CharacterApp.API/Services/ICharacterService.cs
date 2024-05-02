using CharacterApp.Models;
using CharacterApp.Models.DTO;

namespace CharacterApp.Services;

public interface ICharacterService
{
    Task<List<CharacterOnlyDTO>> GetCharactersAsync(int offset, int limit, string search);
    Task<Character?> GetCharacterByIdAsync(int id);
    Task<Character> CreateCharacterAsync(CharacterOnlyDTO newCharacter);
    Task<Character?> UpdateCharacterAsync(CharacterOnlyDTO characterToUpdate);
    Task<Character?> DeleteCharacterAsync(int id);
    Task<Character> PurchaseItemsAsync(OrderDTO order);
}