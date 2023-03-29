using System.ComponentModel.DataAnnotations;

namespace ImpactSpace.Core.Organizations;

public class UpdateOrganizationDto
{
    [Required]
    [StringLength(OrganizationConstants.MaxNameLength)]
    public string Name { get; set; }
    
    [StringLength(OrganizationConstants.MaxDescriptionLength)]
    public string Description { get; set; }
}