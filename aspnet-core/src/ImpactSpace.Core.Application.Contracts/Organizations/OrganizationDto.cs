using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace ImpactSpace.Core.Organizations;

public class OrganizationDto : AuditedEntityDto<Guid>
{
    [Required]
    [StringLength(OrganizationConstants.MaxNameLength)]
    public string Name { get; set; }
    
    [StringLength(OrganizationConstants.MaxDescriptionLength)]
    public string Description { get; set; }
    
    public Guid TenantId { get; set; }
}