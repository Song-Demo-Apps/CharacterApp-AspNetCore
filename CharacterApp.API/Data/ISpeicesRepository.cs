using CharacterApp.Models;

namespace CharacterApp.Data;

public interface ISpeicesRepository 
{
    public Task<List<Speices>> GetAllSpeicesAsync(int offset, int limit);
    public Task<Speices?> GetSpeicesByIdAsync(int id);
    public Task<Speices?> UpdateSpeicesAsync(Speices speices);
    public Task<Speices> CreateSpeicesAsync(Speices speices);
    public Task<Speices> DeleteSpeicesAsync(Speices speices);
}