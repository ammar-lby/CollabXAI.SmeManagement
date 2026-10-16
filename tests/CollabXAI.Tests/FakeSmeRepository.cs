using CollabXAI.Application.Interfaces;
using CollabXAI.Domain.Entities;

namespace CollabXAI.Tests;

// a fake repository that keeps smes in a list instead of a database,
// so the service tests are fast and need no setup
public class FakeSmeRepository : ISmeRepository 
{
    public List<Sme> Smes { get; } = new();

    public Task<List<Sme>> GetAllAsync()
    {
        return Task.FromResult(Smes.ToList());
    }

    public Task<Sme?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(Smes.FirstOrDefault(s => s.Id == id));
    }

    public Task<bool> EmailExistsAsync(string email, Guid? excludeId = null)
    {
        return Task.FromResult(Smes.Any(s => s.ContactEmail == email && s.Id != excludeId));
    }

    public Task AddAsync(Sme sme)
    {
        Smes.Add(sme);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Sme sme)
    {
        return Task.CompletedTask; 
    }

    public Task DeleteAsync(Sme sme)
    {
        Smes.Remove(sme);
        return Task.CompletedTask;
    }
}
