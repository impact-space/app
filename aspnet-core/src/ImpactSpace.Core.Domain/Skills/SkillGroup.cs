using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace ImpactSpace.Core.Skills;

/// <summary>
/// Represents a group of skills.
/// </summary>
public class SkillGroup : Entity<Guid>
{
    /// <summary>
    /// Gets or sets the name of the skill group.
    /// </summary>
    public string Name { get; private set; }
    
    /// <summary>
    /// Gets or sets the description of the skill group.
    /// </summary>
    public string Description { get; set; }
    
    /// <summary>
    /// Gets the skills in the skill group.
    /// </summary>
    public List<Skill> Skills { get; private set; } = new();

    private SkillGroup()
    {
        /* This constructor is for deserialization / ORM purpose */
    }
    
    internal SkillGroup(
        Guid id,
        [NotNull] string name,
        [CanBeNull] string description = null)
        : base(id)
    {
        SetName(name);
        Description = description;
    }

    /// <summary>
    /// Changes the name of the skill group.
    /// </summary>
    /// <param name="name">The new name of the skill group.</param>
    /// <returns>The updated skill group.</returns>
    internal SkillGroup ChangeName([NotNull] string name)
    {
        SetName(name);
        return this;
    }

    /// <summary>
    /// Adds a new skill to the skill group.
    /// </summary>
    /// <param name="skill">The skill to be added.</param>
    /// <returns>The added skill.</returns>
    internal Skill AddSkill([NotNull] Skill skill)
    {
        Check.NotNull(skill, nameof(skill));
        Skills.Add(skill);
        return skill;
    }
    
    /// <summary>
    /// Removes a skill from the skill group.
    /// </summary>
    /// <param name="skill">The skill to be removed.</param>
    internal void RemoveSkill([NotNull] Skill skill)
    {
        Check.NotNull(skill, nameof(skill));
        Skills.Remove(skill);
    }

    private void SetName([NotNull] string name)
    {
        Name = Check.NotNullOrWhiteSpace(
            name, 
            nameof(name), 
            maxLength: SkillGroupConsts.MaxNameLength
        );
    }
}