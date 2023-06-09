﻿using AutoMapper;
using ImpactSpace.Core.Challenges;
using ImpactSpace.Core.Skills;

namespace ImpactSpace.Core;

public class CoreApplicationAutoMapperProfile : Profile
{
    public CoreApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        
        CreateMap<Skill, SkillDto>();
        CreateMap<SkillGroup, SkillGroupDto>();
        CreateMap<SkillGroupDto, SkillGroupUpdateDto>();
        CreateMap<SkillDto, SkillUpdateDto>();
        
        CreateMap<Challenge, ChallengeDto>();
        CreateMap<ChallengeDto, ChallengeUpdateDto>();
        CreateMap<ChallengeCreateDto, Challenge>();
    }
}
