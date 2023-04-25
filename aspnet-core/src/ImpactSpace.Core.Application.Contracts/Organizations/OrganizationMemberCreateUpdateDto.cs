using System;
using System.ComponentModel.DataAnnotations;

namespace ImpactSpace.Core.Organizations;

public class OrganizationMemberCreateUpdateDto
{
    [Required]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Phone]
    public string PhoneNumber { get; set; }
    
    public Guid OrganizationId { get; set; }
}