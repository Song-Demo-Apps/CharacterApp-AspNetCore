using System.ComponentModel.DataAnnotations;

namespace CharacterApp.Models;

public class Speices {
    public int? Id { get; set; }
    [Required]
    public string Name { get; set; } = "";
    public string? Description { get; set; }
}