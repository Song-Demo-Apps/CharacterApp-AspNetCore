namespace CharacterApp.Models.DTO;

public class CharacterOnlyDTO
{
    public CharacterOnlyDTO()
    {
        this.Age = CalcuateAge();
    }
    //mapping constructor between DTO and Model class
    public CharacterOnlyDTO(Character character)
    {
        Id = character.Id ?? null;
        Name = character.Name ?? "";
        DoB = character.DoB;
        Bio = character.Bio;
        CharacterSpeices = character.CharacterSpeices;
        Age = CalcuateAge();
    }

    public int? Id { get; set; }
    public string Name { get; set; } = "";
    public decimal Money { get; set; } = 0.0m;
    public DateOnly DoB { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public int Age { get; private set; }
    public string? Bio { get; set; }
    public Speices CharacterSpeices { get; set; } = new();

    private int CalcuateAge()
    {
        return (int) ((DateTime.Today - this.DoB.ToDateTime(new TimeOnly(0, 0, 0))).TotalDays / 365.2425);
    }
}