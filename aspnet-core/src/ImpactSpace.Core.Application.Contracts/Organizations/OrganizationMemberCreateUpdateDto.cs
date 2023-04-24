using System;

namespace ImpactSpace.Core.Organizations;

public class OrganizationMemberCreateUpdateDto
{
    public string Name { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public Guid OrganizationId { get; set; }
}