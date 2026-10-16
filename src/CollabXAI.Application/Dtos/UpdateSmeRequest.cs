using System.ComponentModel.DataAnnotations;

namespace CollabXAI.Application.Dtos;

public class UpdateSmeRequest
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string OrganisationName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(254)] //valid max length of a valid email address, email standard across platforms 
    public string ContactEmail { get; set; } = string.Empty;
}
