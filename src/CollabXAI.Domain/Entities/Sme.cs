namespace CollabXAI.Domain.Entities;

public class Sme
{
    public Guid Id { get; set; }

    public string OrganisationName { get; set; } = string.Empty;

    public string ContactEmail { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
