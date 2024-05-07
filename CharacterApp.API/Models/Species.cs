using System.ComponentModel.DataAnnotations;

namespace CharacterApp.Models;

public class Species {
    public int? Id { get; set; }
    public string Name { get; set; } = "";
    public string? Description { get; set; }
}