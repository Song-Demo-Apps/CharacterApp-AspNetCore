namespace CharacterApp.Models;

public class CharacterItem
{
    public int? Id { get; set; }
    public Item Item { get; set; } = new();
    public Character Character { get; set; } = new();
    public int Quantity { get; set; } = 0;
}