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
        Money = character.Money;
        Bio = character.Bio;
        CharacterSpecies = character.CharacterSpecies;
        Age = CalcuateAge();
    }

    public int? Id { get; set; }
    public string? Name { get; set; }
    public decimal? Money { get; set; }
    public DateOnly? DoB { get; set; }
    public int? Age { get; private set; }
    public string? Bio { get; set; }
    public Species? CharacterSpecies { get; set; }

    private int? CalcuateAge()
    {   if(this.DoB is null) return null;
        DateOnly dob = (DateOnly) this.DoB; 
        return (int) ((DateTime.Today - dob.ToDateTime(new TimeOnly(0, 0, 0))).TotalDays / 365.2425);
    }
}