using System.ComponentModel.DataAnnotations;
using CharacterApp.Models.DTO;
using Microsoft.Identity.Client;

namespace CharacterApp.Models;

public class Character
{
    public Character() {}

    //mapping constructor between DTO and Model class
    public Character(CharacterOnlyDTO character)
    {
        Id = character.Id ?? null;
        Name = character.Name ?? "";
        DoB = character.DoB ?? DateOnly.FromDateTime(DateTime.Today);
        Bio = character.Bio;
        CharacterSpeices = character.CharacterSpeices ?? new();
    }

    [Range(0, int.MaxValue)]
    public int? Id { get; set; }
    
    [Required]
    public string Name { get; set; } = "";
    public decimal Money { get; set; } = 0.0m;
    public DateOnly DoB { get; set; }
    public string? Bio { get; set; }
    public Speices CharacterSpeices { get; set; } = new();
    public List<CharacterItem> CharacterItems { get; set; } = new();
}