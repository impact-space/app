using System;
using ImpactSpace.Core.Common;
using ImpactSpace.Core.Skills;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace ImpactSpace.Core.Organizations;

/// <summary>
/// Represents a skill possessed by a member of an organization.
/// </summary>
public class OrganizationMemberSkill : Entity, IMultiTenant
{
    /// <summary>
    /// The ID of the organization member who possesses the skill.
    /// </summary>
    public Guid OrganizationMemberId { get; protected set; }

    /// <summary>
    /// The organization member who possesses the skill.
    /// </summary>
    public virtual OrganizationMember OrganizationMember { get; protected set; }

    /// <summary>
    /// The ID of the skill possessed by the organization member.
    /// </summary>
    public Guid SkillId { get; protected set; }

    /// <summary>
    /// The skill possessed by the organization member.
    /// </summary>
    public virtual Skill Skill { get; protected set; }

    /// <summary>
    /// The proficiency level of the organization member in the skill.
    /// </summary>
    public ProficiencyLevel ProficiencyLevel { get; private set; }

    /// <summary>
    /// The ID of the tenant to which this organization member skill belongs.
    /// </summary>
    public Guid? TenantId { get; set; }

    /// <summary>
    /// Private constructor for deserialization / ORM purposes.
    /// </summary>
    protected OrganizationMemberSkill()
    {
        
    }

    /// <summary>
    /// Creates a new instance of the <see cref="OrganizationMemberSkill"/> class with the specified values.
    /// </summary>
    /// <param name="organizationMemberId">The ID of the organization member who possesses the skill.</param>
    /// <param name="skillId">The ID of the skill possessed by the organization member.</param>
    /// <param name="proficiencyLevel">The proficiency level of the organization member in the skill.</param>
    internal OrganizationMemberSkill(Guid organizationMemberId, Guid skillId,
        ProficiencyLevel proficiencyLevel)
    {
        if (organizationMemberId == Guid.Empty)
        {
            throw new ArgumentException("Organization member ID cannot be empty.", nameof(organizationMemberId));
        }
        
        if (skillId == Guid.Empty)
        {
            throw new ArgumentException("Skill ID cannot be empty.", nameof(skillId));
        }
        
        OrganizationMemberId = organizationMemberId;
        SkillId = skillId;
        SetProficiencyLevel(proficiencyLevel);
    }

    /// <summary>
    /// Changes the proficiency level of the organization member in the skill.
    /// </summary>
    /// <param name="proficiencyLevel">The new proficiency level of the organization member in the skill.</param>
    /// <returns>The updated organization member skill.</returns>
    public OrganizationMemberSkill ChangeProficiencyLevel(ProficiencyLevel proficiencyLevel)
    {
        SetProficiencyLevel(proficiencyLevel);
        return this;
    }

    /// <summary>
    /// Sets the proficiency level of the organization member in the skill.
    /// </summary>
    /// <param name="proficiencyLevel">The proficiency level of the organization member in the skill.</param>
    private void SetProficiencyLevel(ProficiencyLevel proficiencyLevel)
    {
        ProficiencyLevel = proficiencyLevel;
    }

    public override object[] GetKeys()
    {
        return new object[] { OrganizationMemberId, SkillId };
    }
}