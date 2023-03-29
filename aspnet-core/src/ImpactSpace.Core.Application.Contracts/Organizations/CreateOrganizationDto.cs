using System;
using System.ComponentModel.DataAnnotations;

namespace ImpactSpace.Core.Organizations;

public class CreateOrganizationDto
{
    [Required]
    [StringLength(OrganizationConstants.MaxNameLength)]
    public string Name { get; set; }
    
    [StringLength(OrganizationConstants.MaxDescriptionLength)]
    public string Description { get; set; }
}