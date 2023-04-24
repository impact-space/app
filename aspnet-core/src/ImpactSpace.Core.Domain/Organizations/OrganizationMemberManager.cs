using System;
using System.Threading.Tasks;
using ImpactSpace.Core.Common;
using Volo.Abp.Domain.Services;

namespace ImpactSpace.Core.Organizations;

public class OrganizationMemberManager : DomainService
{
    private readonly IOrganizationMemberRepository _organizationMemberRepository;

    public OrganizationMemberManager(IOrganizationMemberRepository organizationMemberRepository)
    {
        _organizationMemberRepository = organizationMemberRepository;
    }
    
    public async Task<OrganizationMember> AddOrEditSkillAsync(Guid memberId, Guid skillId, ProficiencyLevel proficiencyLevel)
    {
        var organizationMember = await _organizationMemberRepository.GetAsync(memberId);
        organizationMember.AddOrEditSkill(skillId, proficiencyLevel);
        return organizationMember;
    }

    public async Task<OrganizationMember> RemoveSkillAsync(Guid memberId, Guid skillId)
    {
        var organizationMember = await _organizationMemberRepository.GetAsync(memberId);
        organizationMember.RemoveSkill(skillId);
        return organizationMember;
    }
}