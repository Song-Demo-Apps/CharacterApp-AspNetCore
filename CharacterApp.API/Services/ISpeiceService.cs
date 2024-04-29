using CharacterApp.Models;

namespace CharacterApp.Services;

public interface ISpeicesService
{
    public Task<List<Speices>> GetAllSpeicesAsync(int offset = 0, int limit = 100);
    public Task<Speices?> GetSpeicesByIdAsync(int id);
    public Task<Speices> UpdateSpeicesAsync(Speices speices);
    public Task<Speices> CreateSpeicesAsync(Speices speices);
    public Task<Speices?> DeleteSpeicesAsync(int id);

}