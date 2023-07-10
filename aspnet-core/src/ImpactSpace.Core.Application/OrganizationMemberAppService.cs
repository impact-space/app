using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        GetOrganizationMemberListDto, 
        OrganizationMemberCreateUpdateDto, 
        OrganizationMemberCreateUpdateDto>, 
    IOrganizationMemberAppService
{
    private readonly OrganizationMemberManager _organizationMemberManager;
    private readonly IRepository<Organization, Guid> _organizationRepository;
    private readonly IOrganizationMemberRepository _organizationMemberRepository;
    private readonly IRepository<OrganizationMemberSkill> _organizationMemberSkillRepository;

    public OrganizationMemberAppService(
        IOrganizationMemberRepository repository,
        OrganizationMemberManager organizationMemberManager, 
        IRepository<Organization, Guid> organizationRepository, 
        IRepository<OrganizationMemberSkill> organizationMemberSkillRepository)
        : base(repository)
    {
        _organizationMemberManager = organizationMemberManager;
        _organizationRepository = organizationRepository;
        _organizationMemberSkillRepository = organizationMemberSkillRepository;
        _organizationMemberRepository = repository;
    }

    public override async Task<PagedResultDto<OrganizationMemberDto>> GetListAsync(GetOrganizationMemberListDto input)
    {
        if (input.Sorting.IsNullOrWhiteSpace())
        {
            input.Sorting = nameof(OrganizationMember.Name);
        }
        
        var organizationId = (await _organizationRepository.FirstOrDefaultAsync(o => o.TenantId == CurrentTenant.Id)).Id;
        
        var organizationMembers = await _organizationMemberRepository.GetListAsync(
            organizationId,
            input.SkipCount,
            input.MaxResultCount,
            input.Sorting,
            input.Filter
        );
        
        var totalCount = input.Filter == null
            ? await Repository.CountAsync(member => member.OrganizationId == organizationId)
            : await Repository.CountAsync(member => 
                (input.Filter == null || member.Name.Contains(input.Filter)) 
                && member.OrganizationId == organizationId);
        
        return new PagedResultDto<OrganizationMemberDto>( 
            totalCount,
            ObjectMapper.Map<List<OrganizationMember>, List<OrganizationMemberDto>>(organizationMembers)
        );
    }

    public override async Task<OrganizationMemberDto> CreateAsync(OrganizationMemberCreateUpdateDto input)
    {
        var organizationId = (await _organizationRepository.FirstOrDefaultAsync(o => o.TenantId == CurrentTenant.Id)).Id;

        // Modify the input object to set the OrganizationId before creating the entity
        input.OrganizationId = organizationId;

        return await base.CreateAsync(input);
    }

    public override async Task<OrganizationMemberDto> UpdateAsync(Guid id, OrganizationMemberCreateUpdateDto input)
    {
        var organizationId = (await _organizationRepository.FirstOrDefaultAsync(o => o.TenantId == CurrentTenant.Id)).Id;

        // Modify the input object to set the OrganizationId before updating the entity
        input.OrganizationId = organizationId;

        return await base.UpdateAsync(id, input);
    }

    public async Task<OrganizationMemberSkillDto> AddOrEditSkillAsync(Guid memberId, Guid skillId,
        ProficiencyLevel proficiencyLevel)
    {
        var skill = await _organizationMemberSkillRepository.FindAsync(x =>
            x.OrganizationMemberId == memberId && x.SkillId == skillId);

        if (skill == null)
        {
            skill = _organizationMemberManager.CreateSkill(memberId, skillId, proficiencyLevel);
            
            await _organizationMemberSkillRepository.InsertAsync(skill, true);
        }
        else
        {
            skill.ChangeProficiencyLevel(proficiencyLevel);
            
            await _organizationMemberSkillRepository.UpdateAsync(skill, true);
        }
        
        return ObjectMapper.Map<OrganizationMemberSkill, OrganizationMemberSkillDto>(skill);
    }

    public async Task RemoveSkillAsync(Guid memberId, Guid skillId)
    {
        await _organizationMemberSkillRepository.DeleteAsync(x => x.OrganizationMemberId == memberId && x.SkillId == skillId);
    }

    public async Task<OrganizationMemberDto> GetDetailsAsync(Guid memberId)
    {
        return ObjectMapper.Map<OrganizationMember,OrganizationMemberDto>(await _organizationMemberRepository.GetAsync(memberId));
    }

    public async Task<OrganizationMemberSkillDto> GetSkillAsync(Guid memberId, Guid skillId)
    {
        return ObjectMapper.Map<OrganizationMemberSkill, OrganizationMemberSkillDto>(
            await _organizationMemberSkillRepository.GetAsync(x=>x.OrganizationMemberId == memberId && x.SkillId == skillId)
        );
    }
    
    public async Task<List<OrganizationMemberSkillDto>> GetSkillsAsync(Guid memberId)
    {
        return ObjectMapper.Map<List<OrganizationMemberSkill>, List<OrganizationMemberSkillDto>>(
            await _organizationMemberSkillRepository.GetListAsync(x=>x.OrganizationMemberId == memberId)
        );
    }
}