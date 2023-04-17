using AutoMapper;

namespace ImpactSpace.Core.Organizations;

public class OrganizationProfileMappingProfile : Profile
{
    public OrganizationProfileMappingProfile()
    {
        CreateMap<OrganizationProfile, OrganizationProfileDto>();
        CreateMap<OrganizationProfileDto, OrganizationProfileCreateUpdateDto>();
        CreateMap<OrganizationProfileCreateUpdateDto, OrganizationProfile>();
    }
}