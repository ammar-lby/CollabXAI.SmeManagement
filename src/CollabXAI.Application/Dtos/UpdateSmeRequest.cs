using System.ComponentModel.DataAnnotations;

namespace CollabXAI.Application.Dtos;

public class UpdateSmeRequest
{
    [Required]
    [StringLength(200, MinimumLength = 2)]
    public string OrganisationName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(254)]
    public string ContactEmail { get; set; } = string.Empty;
}
