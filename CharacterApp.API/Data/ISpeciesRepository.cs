using CharacterApp.Models;

namespace CharacterApp.Data;

public interface ISpeciesRepository 
{
    public Task<List<Species>> GetAllSpeciesAsync(int offset, int limit);
    public Task<Species?> GetSpeciesByIdAsync(int id);
    public Task<Species?> UpdateSpeciesAsync(Species species);
    public Task<Species> CreateSpeciesAsync(Species species);
    public Task<Species> DeleteSpeciesAsync(Species species);
}