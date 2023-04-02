using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace ImpactSpace.Core.Skills;

/// <summary>
/// Represents a group of skills.
/// </summary>
public class SkillGroup : AggregateRoot<Guid>
{
    /// <summary>
    /// Gets or sets the name of the skill group.
    /// </summary>
    public virtual string Name { get; private set; }
    
    /// <summary>
    /// Gets or sets the description of the skill group.
    /// </summary>
    public virtual string Description { get; private set; }
    
    /// <summary>
    /// Gets or sets the skills in the skill group.
    /// </summary>
    public virtual ICollection<Skill> Skills { get; private set; }

    /// <summary>
    /// This constructor is for deserialization / ORM purposes
    /// </summary>
    protected SkillGroup()
    {
        
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="SkillGroup"/> class.
    /// </summary>
    /// <param name="id">The identifier of the skill group.</param>
    /// <param name="name">The name of the skill group.</param>
    /// <param name="description">The description of the skill group.</param>
    internal SkillGroup(
        Guid id,
        [NotNull] string name,
        [CanBeNull] string description = null)
        : base(id)
    {
        SetName(name);
        Description = description;

        Skills = new Collection<Skill>();
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
    /// Changes the description of the skill group.
    /// </summary>
    /// <param name="description">The new description of the skill group.</param>
    /// <returns>The updated skill group.</returns>
    internal SkillGroup ChangeDescription(string description)
    {
        Description = description;
        return this;
    }

    /// <summary>
    /// Adds a new skill to the skill group.
    /// </summary>
    /// <param name="skill">The skill to be added.</param>
    internal void AddSkill([NotNull] Skill skill)
    {
        Check.NotNull(skill, nameof(skill));
        Skills.Add(skill);
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

    /// <summary>
    /// Sets the name for the skill group.
    /// </summary>
    /// <param name="name">The name of the skill group.</param>
    private void SetName([NotNull] string name)
    {
        Name = Check.NotNullOrWhiteSpace(
            name, 
            nameof(name), 
            maxLength: SkillGroupConstants.MaxNameLength
        );
    }
    
    /// <summary>
    /// Sets the description for the skill group.
    /// </summary>
    /// <param name="description">The description of the skill group.</param>
    private void SetDescription([CanBeNull] string description)
    {
        Description = description;
    }
}