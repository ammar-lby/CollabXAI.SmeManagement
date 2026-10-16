using CollabXAI.Application.Dtos;
using CollabXAI.Application.Interfaces;
using CollabXAI.Domain.Entities;
using CollabXAI.Domain.Exceptions;

namespace CollabXAI.Application.Services;

public class SmeService : ISmeService
{
    private readonly ISmeRepository _repository;

    public SmeService(ISmeRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<SmeResponse>> GetAllAsync()
    {
        var smes = await _repository.GetAllAsync();
        return smes.Select(ToResponse).ToList();
    }

    public async Task<SmeResponse> GetByIdAsync(Guid id)
    {
        var sme = await _repository.GetByIdAsync(id) ?? throw new SmeNotFoundException(id);
        return ToResponse(sme);
    }

    public async Task<SmeResponse> CreateAsync(CreateSmeRequest request)
    {
        var email = request.ContactEmail.Trim().ToLowerInvariant();

        // business rule: two smes can't share an email
        if (await _repository.EmailExistsAsync(email))
        {
            throw new DuplicateEmailException(email);
        }

        var sme = new Sme
        {
            Id = Guid.NewGuid(),
            OrganisationName = request.OrganisationName.Trim(),
            ContactEmail = email,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(sme);
        return ToResponse(sme);
    }

    public async Task<SmeResponse> UpdateAsync(Guid id, UpdateSmeRequest request)
    {
        var sme = await _repository.GetByIdAsync(id) ?? throw new SmeNotFoundException(id);
        var email = request.ContactEmail.Trim().ToLowerInvariant();

        if (await _repository.EmailExistsAsync(email, id))
        {
            throw new DuplicateEmailException(email);
        }

        sme.OrganisationName = request.OrganisationName.Trim();
        sme.ContactEmail = email;
        sme.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(sme);
        return ToResponse(sme);
    }

    public async Task DeleteAsync(Guid id)
    {
        var sme = await _repository.GetByIdAsync(id) ?? throw new SmeNotFoundException(id);
        await _repository.DeleteAsync(sme);
    }

    private static SmeResponse ToResponse(Sme sme)
    {
        return new SmeResponse
        {
            Id = sme.Id,
            OrganisationName = sme.OrganisationName,
            ContactEmail = sme.ContactEmail,
            CreatedAt = sme.CreatedAt,
            UpdatedAt = sme.UpdatedAt
        };
    }
}
