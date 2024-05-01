using System.ComponentModel.DataAnnotations;

namespace CharacterApp.Models.DTO;
public class OrderDTO
{
    [Required]
    public int CharacterId { get; set; }
    public List<LineItemDTO>? ItemsToPurchase { get; set; } = new();
    public List<LineItemDTO>? ItemsToSell { get; set; } = new();

}