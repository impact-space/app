using AutoMapper;

namespace ImpactSpace.Core.Organizations;

public class OrganizationMemberMappingProfile : Profile
{
    public OrganizationMemberMappingProfile()
    {
        CreateMap<OrganizationMember, OrganizationMemberDto>();
        CreateMap<OrganizationMemberDto, OrganizationMemberCreateUpdateDto>();
        CreateMap<OrganizationMemberCreateUpdateDto, OrganizationMember>();
        CreateMap<OrganizationMemberSkill, OrganizationMemberSkillDto>();
        CreateMap<OrganizationMemberSkillDto, OrganizationMemberSkill>();
        
    }
}