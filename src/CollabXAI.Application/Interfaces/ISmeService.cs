using CollabXAI.Application.Dtos;

namespace CollabXAI.Application.Interfaces;

// what the controller can ask for. the controller only knows this interface, not the real SmeService,
// so the service can be swapped or faked in tests
public interface ISmeService
{
    Task<List<SmeResponse>> GetAllAsync();

    Task<SmeResponse> GetByIdAsync(Guid id);

    Task<SmeResponse> CreateAsync(CreateSmeRequest request);

    Task<SmeResponse> UpdateAsync(Guid id, UpdateSmeRequest request);

    Task DeleteAsync(Guid id);
}
