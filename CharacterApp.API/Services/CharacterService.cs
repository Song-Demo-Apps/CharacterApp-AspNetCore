using System.Text.Json;
using CharacterApp.Data;
using CharacterApp.Models;
using CharacterApp.Models.DTO;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;

namespace CharacterApp.Services;

public class CharacterService : ICharacterService
{
    private readonly ICharacterRepository _characterRepo;
    private readonly ISpeicesRepository _speicesRepo;
    private readonly IItemRepository _itemRepo;
    private readonly ILogger<CharacterService> _logger;

    public CharacterService(ICharacterRepository characterRepo, ILogger<CharacterService> logger, ISpeicesRepository speicesRepo, IItemRepository itemRepo) => (_characterRepo, _logger, _speicesRepo, _itemRepo) = (characterRepo, logger, speicesRepo, itemRepo);

    public async Task<Character> CreateCharacterAsync(CharacterOnlyDTO newCharacter)
    {
        Character newChar = new Character(newCharacter);
        // Make sure the new character does not have id
        newChar.Id = null;
        if(newCharacter.CharacterSpeices is not null) 
        {
            // Use the mapping constructor I created for this model class
            Speices? speices = await _speicesRepo.GetSpeicesByIdAsync((int) newCharacter.CharacterSpeices.Id!);
            if(speices is null)
            {
                throw new ArgumentException($"Speices with Id {newCharacter.CharacterSpeices.Id} was not found");
            }
            else
            {
                newChar.CharacterSpeices = speices;
            }
        }

        return await _characterRepo.CreateCharacterAsync(newChar);
    }

    public Task<Character?> DeleteCharacterAsync(int id)
    {
        if(id <= 0)
        {
            throw new ArgumentOutOfRangeException("Id cannot be less than or equal to 0");
        }
        return _characterRepo.DeleteCharacterAsync(id);
    }

    public async Task<Character?> GetCharacterByIdAsync(int id)
    {
        if(id <= 0)
        {
            throw new ArgumentOutOfRangeException("Id cannot be less than or equal to 0");
        }
        return await _characterRepo.GetCharacterByIdAsync(id);
    }

    public async Task<List<CharacterOnlyDTO>> GetCharactersAsync(int offset, int limit, string search)
    {
        return await _characterRepo.GetCharactersAsync(offset, limit, search);
    }

    public async Task<Character> PurchaseItemsAsync(OrderDTO order)
    {
        Character? orderingChar = await _characterRepo.GetCharacterByIdAsync(order.CharacterId);
        
        if(orderingChar is null)
        {
            throw new KeyNotFoundException($"The character with Id {order.CharacterId} was not found");
        }

        List<(string, string)> errors = new();
        decimal currentMoney = orderingChar.Money;
        decimal totalCost = 0.0m;
        Dictionary<int, (Item, int)> items = new();
        if(order.ItemsToPurchase is not null)
        {
            foreach(LineItemDTO line in order.ItemsToPurchase)
            {
                Item? item = await _itemRepo.GetItemByIdAsync(line.ItemId);
                if(item is null)
                {
                    errors.Add((line.ItemId.ToString(), $"{line.ItemId} does not exist"));
                }
                else
                {
                    items.Add((int)item.Id!, (item, line.Quantity));
                    totalCost += (decimal)item.Value! * line.Quantity;
                }
            }
        }
        if(order.ItemsToSell is not null)
        {
            foreach(LineItemDTO line in order.ItemsToSell)
            {
                // Check if this character owns the item and they actually have enough to sell
                CharacterItem? invenItem = orderingChar.CharacterItems.FirstOrDefault(i => i.Item.Id == line.ItemId);
                if(invenItem is null)
                {
                    errors.Add((line.ItemId.ToString(), "This character does not own this item"));
                }
                else if(invenItem.Quantity < line.Quantity)
                {
                    errors.Add((line.ItemId.ToString(), $"The character does not have enough of this item to sell {line.Quantity}"));
                }
                else
                {
                    items.Add((int)invenItem.Item.Id!, (invenItem.Item, line.Quantity * -1));
                    totalCost -= (decimal)invenItem.Item.Value! * line.Quantity;
                }
            }
        }
        if(totalCost > currentMoney)
        {
            errors.Add(("cost", $"This character cannot afford this transaction. The total cost is {totalCost}. The character's current balance is {currentMoney}"));
        }
        if(errors.Count > 0)
        {
            throw new ArgumentException(JsonSerializer.Serialize(errors));
        }
        else
        {
            foreach(CharacterItem ownedItem in orderingChar.CharacterItems)
            {
                (Item, int) purchaseItem;
                bool itemInCart = items.TryGetValue((int) ownedItem.Item.Id!, out purchaseItem);

                if(itemInCart)
                {
                    ownedItem.Quantity += purchaseItem.Item2;
                    items.Remove((int) purchaseItem.Item1.Id!);
                }
            }
            foreach(KeyValuePair<int, (Item, int)> remainingOrderLine in items)
            {
                orderingChar.CharacterItems.Add(
                    new CharacterItem
                    { 
                        CharacterId = (int)orderingChar.Id!, 
                        Item = remainingOrderLine.Value.Item1, 
                        Quantity = remainingOrderLine.Value.Item2
                    });
            }
            orderingChar.Money -= totalCost;
            return await _characterRepo.UpdateCharacterAsync(orderingChar);
        }
    }

    public async Task<Character?> UpdateCharacterAsync(CharacterOnlyDTO charDTO)
    {
        if(charDTO.Id is null) {
            throw new ArgumentNullException("Character Id cannot be null");
        }
        Character? character = await _characterRepo.GetCharacterByIdAsync((int) charDTO.Id);
        if(character is null) return character;
        
        character.Name = string.IsNullOrWhiteSpace(charDTO.Name) ? character.Name : charDTO.Name;
        character.Money = charDTO.Money ?? character.Money;
        character.DoB = charDTO.DoB ?? character.DoB;
        character.Bio = string.IsNullOrWhiteSpace(charDTO.Bio) ? character.Bio : charDTO.Bio;
        if(charDTO.CharacterSpeices is not null)
        {
            if(charDTO.CharacterSpeices.Id is null)
            {
                throw new ArgumentNullException("Character Speices Id cannot be null");
            }
            Speices? sp = await _speicesRepo.GetSpeicesByIdAsync((int) charDTO.CharacterSpeices.Id);

            if(sp is not null) 
            {
                character.CharacterSpeices = sp;
            }
        }
        return await _characterRepo.UpdateCharacterAsync(character);
    }
}