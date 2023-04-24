using Volo.Abp.Domain.Services;

namespace ImpactSpace.Core.Organizations;

public class OrganizationMemberManager : DomainService
{
    private readonly IOrganizationMemberRepository _organizationMemberRepository;

    public OrganizationMemberManager(IOrganizationMemberRepository organizationMemberRepository)
    {
        _organizationMemberRepository = organizationMemberRepository;
    }
}