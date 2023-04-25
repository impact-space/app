using System;
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
    Task<OrganizationMemberDto> AddOrEditSkillAsync(Guid memberId, Guid skillId, ProficiencyLevel proficiencyLevel);

    Task<OrganizationMemberDto> RemoveSkillAsync(Guid memberId, Guid skillId);
}