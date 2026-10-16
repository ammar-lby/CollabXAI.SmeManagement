namespace CollabXAI.Domain.Entities;

public class Sme
{
    public Guid Id { get; set; } //globablly unique identifier, random 128-bit , 

    public string OrganisationName { get; set; } = string.Empty;

    public string ContactEmail { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
