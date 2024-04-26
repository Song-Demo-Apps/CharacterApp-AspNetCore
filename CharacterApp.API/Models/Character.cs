namespace CharacterApp.Models;

public class Character
{
    public int? Id { get; set; }
    public string Name { get; set; } = "";
    public DateOnly DoB { get; set; }
    public Speices CharacterSpeices { get; set; } = new();
    public List<CharacterItem> CharacterItems { get; set; } = new();
}