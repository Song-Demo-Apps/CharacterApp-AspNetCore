using CharacterApp.Models;
using CharacterApp.Models.DTO;

namespace CharacterApp.Services;

public class CharacterService : ICharacterService
{
    public Task<CharacterOnlyDTO> CreateCharacter(CharacterOnlyDTO newCharacter)
    {
        throw new NotImplementedException();
    }

    public Task<CharacterOnlyDTO> DeleteCharacter(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Character> GetCharacterById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<CharacterOnlyDTO>> GetCharacters(int offset, int limit, string search)
    {
        throw new NotImplementedException();
    }

    public Task<Character> PurchaseItems(OrderDTO order)
    {
        throw new NotImplementedException();
    }

    public Task<CharacterOnlyDTO> UpdateCharacter(CharacterOnlyDTO characterToUpdate)
    {
        throw new NotImplementedException();
    }
}