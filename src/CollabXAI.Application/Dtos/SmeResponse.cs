namespace CollabXAI.Application.Dtos;

// what the api sends back to the client. we return this instead of the Sme entity
// so the database model is never exposed directly
public class SmeResponse
{
    public Guid Id { get; set; }

    public string OrganisationName { get; set; } = string.Empty;

    public string ContactEmail { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } //date time for created at and updated at done for traceability, to know when records were created and changed.

    public DateTime? UpdatedAt { get; set; }
}
