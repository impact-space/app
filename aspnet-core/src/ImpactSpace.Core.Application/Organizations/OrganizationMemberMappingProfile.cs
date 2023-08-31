using AutoMapper;

namespace ImpactSpace.Core.Organizations;

public class OrganizationMemberMappingProfile : Profile
{
    public OrganizationMemberMappingProfile()
    {
        CreateMap<OrganizationMember, OrganizationMemberDto>();
        CreateMap<OrganizationMemberDto, OrganizationMemberCreateUpdateDto>();
        CreateMap<OrganizationMemberCreateUpdateDto, OrganizationMember>();
        CreateMap<OrganizationMemberSkill, OrganizationMemberSkillDto>()
            .ForMember(dest => dest.SkillName, opt => opt.MapFrom(src => src.Skill.Name));
        CreateMap<OrganizationMemberSkillDto, OrganizationMemberSkill>();
        
    }
}