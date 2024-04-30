namespace CharacterApp.Models;

public class CharacterItem
{
    public int? Id { get; set; }
    public int CharacterId { get; set; }
    public Item Item { get; set; } = new();
    public int Quantity { get; set; } = 0;
}