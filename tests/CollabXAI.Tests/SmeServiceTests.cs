using CollabXAI.Application.Dtos;
using CollabXAI.Application.Services;
using CollabXAI.Domain.Exceptions;

namespace CollabXAI.Tests;

public class SmeServiceTests
{
    private readonly FakeSmeRepository _repository = new();
    private readonly SmeService _service;

    public SmeServiceTests()
    {
        _service = new SmeService(_repository);
    }

    [Fact] //marks method as a test for fact, similar to @Test in springboot 
    public async Task Create_StoresSmeWithLowercaseEmail()
    {
        var request = new CreateSmeRequest { OrganisationName = "  Acme Bakery ", ContactEmail = "Hello@Acme.com" };

        var result = await _service.CreateAsync(request);

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("Acme Bakery", result.OrganisationName);
        Assert.Equal("hello@acme.com", result.ContactEmail);
        Assert.Single(_repository.Smes);
    }

    // the main business rule: the same email can't be registered twice,
    // even if the letter case is different
    [Fact]
    public async Task Create_WithExistingEmail_ThrowsDuplicateEmailException()
    {
        await _service.CreateAsync(new CreateSmeRequest { OrganisationName = "Acme", ContactEmail = "hello@acme.com" });

        var duplicate = new CreateSmeRequest { OrganisationName = "Other", ContactEmail = "HELLO@acme.com" };

        await Assert.ThrowsAsync<DuplicateEmailException>(() => _service.CreateAsync(duplicate));
        Assert.Single(_repository.Smes);
    }

    [Fact]
    public async Task GetById_WhenMissing_ThrowsSmeNotFoundException()
    {
        await Assert.ThrowsAsync<SmeNotFoundException>(() => _service.GetByIdAsync(Guid.NewGuid()));
    }

    // updating without changing the email must work, an sme shouldn't clash with itself
    [Fact]
    public async Task Update_KeepingOwnEmail_Succeeds()
    {
        var created = await _service.CreateAsync(new CreateSmeRequest { OrganisationName = "Acme", ContactEmail = "hello@acme.com" });

        var updated = await _service.UpdateAsync(created.Id, new UpdateSmeRequest { OrganisationName = "Acme Ltd", ContactEmail = "hello@acme.com" });

        Assert.Equal("Acme Ltd", updated.OrganisationName);
        Assert.NotNull(updated.UpdatedAt);
    }
    [Fact]  
    public async Task Update_ToAnotherSmesEmail_ThrowsDuplicateEmailException()
    {
        await _service.CreateAsync(new CreateSmeRequest { OrganisationName = "First", ContactEmail = "first@x.com" });
        var second = await _service.CreateAsync(new CreateSmeRequest { OrganisationName = "Second", ContactEmail = "second@x.com" });

        var request = new UpdateSmeRequest { OrganisationName = "Second", ContactEmail = "first@x.com" };

        await Assert.ThrowsAsync<DuplicateEmailException>(() => _service.UpdateAsync(second.Id, request));
    }

    [Fact]
    public async Task Delete_RemovesSme()
    {
        var created = await _service.CreateAsync(new CreateSmeRequest { OrganisationName = "Acme", ContactEmail = "hello@acme.com" });

        await _service.DeleteAsync(created.Id);

        Assert.Empty(_repository.Smes);
    }
}
