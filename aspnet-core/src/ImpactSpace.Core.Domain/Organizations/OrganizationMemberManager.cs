using System;
using System.Threading.Tasks;
using ImpactSpace.Core.Common;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace ImpactSpace.Core.Organizations;

public class OrganizationMemberManager : DomainService
{
    private readonly IOrganizationMemberRepository _organizationMemberRepository;
    private readonly IRepository<OrganizationMemberSkill> _organizationMemberSkillRepository;

    public OrganizationMemberManager(IOrganizationMemberRepository organizationMemberRepository, IRepository<OrganizationMemberSkill> organizationMemberSkillRepository)
    {
        _organizationMemberRepository = organizationMemberRepository;
        _organizationMemberSkillRepository = organizationMemberSkillRepository;
    }
    
    public OrganizationMember CreateAsync(
        Guid id, 
        string name, 
        string email, 
        string phone,
        Guid organizationId)
    {
        var organizationMember = new OrganizationMember(
            id,
            name,
            email,
            phone,
            organizationId
        );

        return organizationMember;
    }
    
    public  OrganizationMemberSkill CreateSkill(
        Guid organizationMemberId,
        Guid skillId,
        ProficiencyLevel level)
    {
        var organizationMemberSkill = new OrganizationMemberSkill(
            organizationMemberId,
            skillId,
            level
        );

        return organizationMemberSkill;
    }
}