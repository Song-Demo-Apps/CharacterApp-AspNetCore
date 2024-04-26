namespace CharacterApp.Models;

public class Item
{
    public int? Id { get; set; }
    public string Name { get; set; } = "";

    public string? Description { get; set; }

    public decimal Value { get; set; } = 0.0m;
    public string? ImageUrl { get; set; }

}