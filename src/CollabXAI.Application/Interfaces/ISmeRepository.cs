using CollabXAI.Domain.Entities;

namespace CollabXAI.Application.Interfaces;

// the service only knows this interface, the real database code is in infrastructure
public interface ISmeRepository
{
    Task<List<Sme>> GetAllAsync();

    Task<Sme?> GetByIdAsync(Guid id);

    Task<bool> EmailExistsAsync(string email, Guid? excludeId = null);

    Task AddAsync(Sme sme);

    Task UpdateAsync(Sme sme);

    Task DeleteAsync(Sme sme);
}
