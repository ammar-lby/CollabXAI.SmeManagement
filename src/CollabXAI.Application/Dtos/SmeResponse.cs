namespace CollabXAI.Application.Dtos;

public class SmeResponse
{
    public Guid Id { get; set; }

    public string OrganisationName { get; set; } = string.Empty;

    public string ContactEmail { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
