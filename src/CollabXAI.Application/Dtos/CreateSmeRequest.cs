using System.ComponentModel.DataAnnotations;

namespace CollabXAI.Application.Dtos;

// what the client sends when registering. the attributes are the validation rules,
// asp.net checks them and returns a 400 automatically if they fail
public class CreateSmeRequest
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string OrganisationName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(254)]
    public string ContactEmail { get; set; } = string.Empty;
}
