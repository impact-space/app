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
        OrganizationMemberManager organizationMemberManager,
        IMapper mapper)
        : base(repository)
    {
        _organizationMemberManager = organizationMemberManager;
    }
    
    public Task<OrganizationMemberDto> AddOrEditSkillAsync(Guid memberId, Guid skillId, ProficiencyLevel proficiencyLevel)
    {
        throw new NotImplementedException();
    }

    public Task<OrganizationMemberDto> RemoveSkillAsync(Guid memberId, Guid skillId)
    {
        throw new NotImplementedException();
    }
}