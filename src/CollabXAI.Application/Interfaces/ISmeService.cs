using CollabXAI.Application.Dtos;

namespace CollabXAI.Application.Interfaces;

public interface ISmeService
{
    Task<List<SmeResponse>> GetAllAsync();

    Task<SmeResponse> GetByIdAsync(Guid id);

    Task<SmeResponse> CreateAsync(CreateSmeRequest request);

    Task<SmeResponse> UpdateAsync(Guid id, UpdateSmeRequest request);

    Task DeleteAsync(Guid id);
}
