using System.Text.Json;
using CharacterApp.Data;
using CharacterApp.Models;
using CharacterApp.Models.DTO;

namespace CharacterApp.Services;

public class CharacterService : ICharacterService
{
    private readonly ICharacterRepository _characterRepo;
    private readonly ISpeciesRepository _speciesRepo;
    private readonly IItemRepository _itemRepo;
    private readonly ILogger<CharacterService> _logger;

    public CharacterService(ICharacterRepository characterRepo, ILogger<CharacterService> logger, ISpeciesRepository speciesRepo, IItemRepository itemRepo) => (_characterRepo, _logger, _speciesRepo, _itemRepo) = (characterRepo, logger, speciesRepo, itemRepo);

    public async Task<Character> CreateCharacterAsync(CharacterOnlyDTO newCharacter)
    {
        Character newChar = new Character(newCharacter);
        // Make sure the new character does not have id
        newChar.Id = null;
        if(newCharacter.CharacterSpecies is not null) 
        {
            Species? newSpecies;
            if(newCharacter.CharacterSpecies.Id is null) {
                newSpecies = await _speciesRepo.CreateSpeciesAsync(newCharacter.CharacterSpecies);
            }
            else {
                // Use the mapping constructor I created for this model class
                newSpecies = await _speciesRepo.GetSpeciesByIdAsync((int)newCharacter.CharacterSpecies.Id);

            }
            if(newSpecies is null)
            {
                throw new ArgumentException($"Species with Id {newCharacter.CharacterSpecies.Id} was not found");
            }
            else
            {
                newChar.CharacterSpecies = newSpecies;
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

    /// <summary>
    /// Purchase items for a character
    /// </summary>
    /// <param name="order">The order containing the items to purchase</param>
    /// <returns>The updated character</returns>
    /// <exception cref="ArgumentException">Thrown if there are errors in the order</exception>
    public async Task<Character> PurchaseItemsAsync(OrderDTO order)
    {
        // Get the character making the order
        Character? orderingChar = await _characterRepo.GetCharacterByIdAsync(order.CharacterId);

        // Check if the character exists
        if(orderingChar is null)
        {
            throw new KeyNotFoundException($"The character with Id {order.CharacterId} was not found");
        }

        // List to store errors in the order
        List<(string, string)> errors = new();

        // The current money the character has
        decimal currentMoney = orderingChar.Money;

        // The total cost of the items to purchase
        decimal totalCost = 0.0m;

        // Dictionary to store the items to purchase and their quantities
        Dictionary<int, (Item, int)> items = new();

        // Loop through each item to purchase, adding them to the dictionary and calculating the total cost
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

        // Loop through each item to sell, checking if the character owns the item and if they have enough to sell
        if(order.ItemsToSell is not null)
        {
            foreach(LineItemDTO line in order.ItemsToSell)
            {
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

        // Check if the character can afford the transaction
        if(totalCost > currentMoney)
        {
            errors.Add(("cost", $"This character cannot afford this transaction. The total cost is {totalCost}. The character's current balance is {currentMoney}"));
        }

        // If there are errors, throw an exception with the errors
        if(errors.Count > 0)
        {
            throw new ArgumentException(JsonSerializer.Serialize(errors));
        }
        else
        {
            // Loop through each item the character owns, updating the quantities if the item is in the order
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

            // Loop through each remaining item in the order, adding them to the character's inventory
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

            // Update the character's money and return the updated character
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
        if(charDTO.CharacterSpecies is not null)
        {
            if(charDTO.CharacterSpecies.Id is null)
            {
                throw new ArgumentNullException("Character Species Id cannot be null");
            }
            Species? sp = await _speciesRepo.GetSpeciesByIdAsync((int) charDTO.CharacterSpecies.Id);

            if(sp is not null) 
            {
                character.CharacterSpecies = sp;
            }
        }
        return await _characterRepo.UpdateCharacterAsync(character);
    }
}