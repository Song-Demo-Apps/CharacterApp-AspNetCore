using CharacterApp.Models;
using CharacterApp.Models.DTO;

namespace CharacterApp.Services;

public interface ICharacterService
{
    Task<List<CharacterOnlyDTO>> GetCharacters(int offset, int limit, string search);
    Task<Character> GetCharacterById(int id);
    Task<CharacterOnlyDTO> CreateCharacter(CharacterOnlyDTO newCharacter);
    Task<CharacterOnlyDTO> UpdateCharacter(CharacterOnlyDTO characterToUpdate);
    Task<CharacterOnlyDTO> DeleteCharacter(int id);
    Task<Character> PurchaseItems(OrderDTO order);
}