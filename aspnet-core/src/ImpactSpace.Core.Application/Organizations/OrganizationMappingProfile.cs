using AutoMapper;

namespace ImpactSpace.Core.Organizations;

public class OrganizationMappingProfile: Profile
{
    public OrganizationMappingProfile()
    {
        CreateMap<Organization, OrganizationDto>();
    }
}