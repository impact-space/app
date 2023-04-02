using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ImpactSpace.Core.Organizations;
using ImpactSpace.Core.Projects;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace ImpactSpace.Core.Skills;

/// <summary>
/// Represents a skill that can be associated with members of an organization and/or required by a project.
/// </summary>
public class Skill : AggregateRoot<Guid>
    {
        /// <summary>
        /// Gets the name of the skill.
        /// </summary>
        public virtual string Name { get; private set; }
        
        /// <summary>
        /// Gets or sets the skill group ID to which this skill belongs.
        /// </summary>
        public virtual Guid SkillGroupId { get; private set; }

        /// <summary>
        /// Gets the skills associated with members of an organization.
        /// </summary>
        public virtual ICollection<OrganizationMemberSkill> OrganizationMemberSkills { get; private set; }
        
        /// <summary>
        /// Gets the skills associated with a project.
        /// </summary>
        public virtual ICollection<ProjectSkill> ProjectSkills { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Skill"/> class for deserialization / ORM purposes.
        /// </summary>
        protected Skill()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Skill"/> class.
        /// </summary>
        /// <param name="id">The identifier of the skill.</param>
        /// <param name="name">The name of the skill.</param>
        /// <param name="skillGroupId">The identifier of the skill group</param>
        internal Skill(Guid id, [NotNull] string name, Guid skillGroupId)
            : base(id)
        {
            SetName(name);
            SetSkillGroup(skillGroupId);

            OrganizationMemberSkills = new Collection<OrganizationMemberSkill>();
            ProjectSkills = new Collection<ProjectSkill>();
        }

        /// <summary>
        /// Changes the name of the skill.
        /// </summary>
        /// <param name="name">The new name of the skill.</param>
        /// <returns>The skill object.</returns>
        internal Skill ChangeName([NotNull] string name)
        {
            SetName(name);
            return this;
        }
        
        /// <summary>
        /// Changes the skill group of the skill.
        /// </summary>
        /// <param name="skillGroupId">The new skill group's identifier.</param>
        /// <returns>The skill object.</returns>
        internal Skill ChangeSkillGroup(Guid skillGroupId)
        {
            SetSkillGroup(skillGroupId);
            return this;
        }

        /// <summary>
        /// Sets the Skill's name to the specified value.
        /// </summary>
        /// <param name="name">The new name for the Skill</param>
        /// <exception cref="ArgumentException">Thrown when the specified name is null, whitespace, or exceeds the maximum length allowed.</exception>
        private void SetName([NotNull] string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: SkillConstants.MaxNameLength
            );
        }
        
        /// <summary>
        /// Sets the Skill's skill group ID to the specified value.
        /// </summary>
        /// <param name="skillGroupId"></param>
        private void SetSkillGroup(Guid skillGroupId)
        {
            SkillGroupId = skillGroupId;
        }
    }