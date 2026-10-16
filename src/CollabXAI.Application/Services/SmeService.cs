using CollabXAI.Application.Dtos;
using CollabXAI.Application.Interfaces;
using CollabXAI.Domain.Entities;
using CollabXAI.Domain.Exceptions;

namespace CollabXAI.Application.Services;

// the business logic layer. the controller calls this and this calls the repository.
// it never touches the database or http directly, which keeps it easy to unit test
public class SmeService : ISmeService
{
    private readonly ISmeRepository _repository;

    // the repository is injected (passed in) instead of created here,
    // so in tests we can swap in a fake one
    public SmeService(ISmeRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<SmeResponse>> GetAllAsync()
    {
        var smes = await _repository.GetAllAsync();

        // convert each entity to a response object so we dont expose the db model
        return smes.Select(ToResponse).ToList();
    }

    public async Task<SmeResponse> GetByIdAsync(Guid id)
    {
        // the repository returns null when nothing matches, ?? throw turns that into
        // a not found exception which the api later maps to a 404
        var sme = await _repository.GetByIdAsync(id) ?? throw new SmeNotFoundException(id);
        return ToResponse(sme);
    }

    public async Task<SmeResponse> CreateAsync(CreateSmeRequest request)
    {
        // clean the input first. lowercasing means Hello@x.com and hello@x.com
        // count as the same email, otherwise the duplicate check could be bypassed
        var email = request.ContactEmail.Trim().ToLowerInvariant();

        // business rule: two smes can't share an email
        if (await _repository.EmailExistsAsync(email))
        {
            throw new DuplicateEmailException(email); //comment this out to simulate a test failing to make sure automated tests work
        }

        // the server creates the id and timestamp, the client doesnt get to choose them
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
        // load the existing sme first, so updating a missing one gives a 404
        var sme = await _repository.GetByIdAsync(id) ?? throw new SmeNotFoundException(id);
        var email = request.ContactEmail.Trim().ToLowerInvariant();

        // same rule as create, but we pass the id so the sme's own current email
        // isnt counted as a duplicate (otherwise you couldnt save without changing it)
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
        // check it exists first so deleting something that isnt there returns 404, not a silent success
        var sme = await _repository.GetByIdAsync(id) ?? throw new SmeNotFoundException(id);
        await _repository.DeleteAsync(sme);
    }

    // maps the Sme entity to a SmeResponse so the database model is never exposed to the client,
    // that way the entity and the api response can change separately
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
