using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImpactSpace.Core.Common;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ImpactSpace.Core.Organizations;

public interface IOrganizationMemberAppService: ICrudAppService<
    OrganizationMemberDto, 
    Guid, 
    GetOrganizationMemberListDto, 
    OrganizationMemberCreateUpdateDto, 
    OrganizationMemberCreateUpdateDto>
{
    Task<OrganizationMemberSkillDto> AddOrEditSkillAsync(Guid memberId, Guid skillId, ProficiencyLevel proficiencyLevel);

    Task RemoveSkillAsync(Guid memberId, Guid skillId);
    
    Task<OrganizationMemberDto> GetDetailsAsync(Guid memberId);
    
    Task<OrganizationMemberSkillDto> GetSkillAsync(Guid memberId, Guid skillId);
    
    Task<List<OrganizationMemberSkillDto>> GetSkillsAsync(Guid memberId);
}