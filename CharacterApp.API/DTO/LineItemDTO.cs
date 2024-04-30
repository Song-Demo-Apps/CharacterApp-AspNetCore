using System.ComponentModel.DataAnnotations;

namespace CharacterApp.Models.DTO;

public class LineItemDTO
{
    [Required]
    public int ItemId { get; set; }
    public int Quantity { get; set; }
}