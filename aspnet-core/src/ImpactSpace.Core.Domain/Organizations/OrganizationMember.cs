using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ImpactSpace.Core.Common;
using ImpactSpace.Core.Projects;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Identity;

namespace ImpactSpace.Core.Organizations;

public class OrganizationMember : AuditedAggregateRoot<Guid>, IMultiTenant
{
    public string Name { get; private set; }
    
    public string Email { get; private set; }
    
    public PhoneNumber PhoneNumber { get; private set; }

    public Guid? IdentityUserId { get; protected set; }
    
    public virtual IdentityUser IdentityUser { get; protected set; }

    public Guid OrganizationId { get; private set; }
    
    public virtual Organization Organization { get; protected set; }
    
    // ReSharper disable once UnassignedGetOnlyAutoProperty
    public Guid? TenantId { get; }

    public virtual ICollection<OrganizationMemberChallenge> OrganizationMemberChallenges { get; }
    
    public virtual ICollection<OrganizationMemberAction> OrganizationMemberActions { get; }
    
    public virtual ICollection<OrganizationMemberProject> OrganizationMemberProjects { get; }

    public virtual ICollection<OrganizationMemberSkill> OrganizationMemberSkills { get; }
    
    public virtual ICollection<SocialMediaLink> SocialMediaLinks { get; }
    
    public virtual ICollection<Project> OwnedProjects { get; }

    protected OrganizationMember()
    {
        
    }
    
    internal OrganizationMember(
        Guid id, 
        [NotNull] string name, 
        [NotNull] string email, 
        PhoneNumber phone,
        Guid organizationId)
        : base(id)
    {
        SetName(name);
        SetEmail(email);
        SetPhoneNumber(phone);
        SetOrganizationId(organizationId);

        OrganizationMemberActions = new Collection<OrganizationMemberAction>();
        OrganizationMemberProjects = new Collection<OrganizationMemberProject>();
        OrganizationMemberSkills = new Collection<OrganizationMemberSkill>();
        OrganizationMemberChallenges = new Collection<OrganizationMemberChallenge>();
        SocialMediaLinks = new Collection<SocialMediaLink>();
        OwnedProjects = new Collection<Project>();
    }

    
    internal OrganizationMember ChangeName([NotNull] string name)
    {
        SetName(name);
        return this;
    }
    
    internal OrganizationMember ChangeEmail([NotNull] string email)
    {
        SetEmail(email);
        return this;
    }
    
    internal OrganizationMember ChangePhoneNumber([CanBeNull] PhoneNumber phoneNumber)
    {
        SetPhoneNumber(phoneNumber);
        return this;
    }
    
    internal OrganizationMember ChangeOrganization(Guid organizationId)
    {
        OrganizationId = organizationId;
        return this;
    }

    internal void AddOrEditSkill(Guid skillId, ProficiencyLevel proficiencyLevel)
    {
        var existingSkill = OrganizationMemberSkills.FirstOrDefault(x => x.SkillId == skillId);

        if (existingSkill != null)
        {
            existingSkill.ChangeProficiencyLevel(proficiencyLevel);
        }
        else
        {
            var skill = new OrganizationMemberSkill(Id, skillId, proficiencyLevel);
            OrganizationMemberSkills.Add(skill);
        }
    }

    internal void RemoveSkill(Guid skillId)
    {
        var skill = OrganizationMemberSkills.FirstOrDefault(x => x.SkillId == skillId);
        if (skill != null)
        {
            OrganizationMemberSkills.Remove(skill);
        }
    }
    
    private void SetName([NotNull] string name)
    {
        Name = Check.NotNullOrWhiteSpace(
            name,
            nameof(name),
            maxLength: OrganizationMemberConsts.MaxNameLength
        );
    }
    
    private void SetEmail([CanBeNull] string email)
    {
        if (!email.IsNullOrWhiteSpace() && !ValidationHelper.IsValidEmail(email))
        {
            throw new ArgumentException("The provided email is not valid.", nameof(email));
        }

        Email = email;
    }
    
    private void SetPhoneNumber([CanBeNull] PhoneNumber phoneNumber)
    {
        PhoneNumber = phoneNumber;
    }
    
    private void SetOrganizationId(Guid organizationId)
    {
        if (organizationId == Guid.Empty)
        {
            throw new ArgumentException("OrganizationId cannot be empty.");
        }
        
        OrganizationId = organizationId;
    }
}