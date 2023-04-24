using System;
using System.Threading.Tasks;
using AutoMapper;
using ImpactSpace.Core.Common;
using ImpactSpace.Core.Organizations;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ImpactSpace.Core;

public class OrganizationMemberAppService: CrudAppService<
        OrganizationMember, 
        OrganizationMemberDto, 
        Guid, 
        PagedAndSortedResultRequestDto, 
        OrganizationMemberCreateUpdateDto, 
        OrganizationMemberCreateUpdateDto>, 
    IOrganizationMemberAppService
{
    private readonly OrganizationMemberManager _organizationMemberManager;

    public OrganizationMemberAppService(
        IRepository<OrganizationMember, Guid> repository,
        OrganizationMemberManager organizationMemberManager)
        : base(repository)
    {
        _organizationMemberManager = organizationMemberManager;
    }

    public async Task<OrganizationMemberDto> AddOrEditSkillAsync(Guid memberId, Guid skillId, ProficiencyLevel proficiencyLevel)
    {
        var organizationMember = await _organizationMemberManager.AddOrEditSkillAsync(memberId, skillId, proficiencyLevel);
        await Repository.UpdateAsync(organizationMember);
        return ObjectMapper.Map<OrganizationMember, OrganizationMemberDto>(organizationMember);
    }

    public async Task<OrganizationMemberDto> RemoveSkillAsync(Guid memberId, Guid skillId)
    {
        var organizationMember = await _organizationMemberManager.RemoveSkillAsync(memberId, skillId);
        await Repository.UpdateAsync(organizationMember);
        return ObjectMapper.Map<OrganizationMember, OrganizationMemberDto>(organizationMember);
    }
}