using System;
using System.Collections.Generic;
using ImpactSpace.Core.Organizations;
using ImpactSpace.Core.Projects;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace ImpactSpace.Core.Skills;

/// <summary>
/// Represents a skill that can be associated with members of an organization and/or required by a project.
/// </summary>
public class Skill : Entity<Guid>
    {
        /// <summary>
        /// Gets the name of the skill.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets the ID of the parent skill, if any.
        /// </summary>
        public Guid? ParentSkillId { get; private set; }
        
        /// <summary>
        /// Gets or sets the parent skill.
        /// </summary>
        public Skill ParentSkill { get; private set; }
        
        /// <summary>
        /// Gets the child skills of this skill.
        /// </summary>
        public List<Skill> ChildSkills { get; private set; } = new();
        
        /// <summary>
        /// Gets or sets the ID of the skill group to which this skill belongs.
        /// </summary>
        public Guid SkillGroupId { get; private set; }
        
        /// <summary>
        /// Gets or sets the skill group to which this skill belongs.
        /// </summary>
        public SkillGroup SkillGroup { get; private set; }
        
        /// <summary>
        /// Gets the organization member skills associated with this skill.
        /// </summary>
        public List<OrganizationMemberSkill> MemberSkills { get; private set; } = new();
        
        /// <summary>
        /// Gets the project skills associated with this skill.
        /// </summary>
        public List<ProjectSkill> ProjectSkills { get; private set; } = new();


        /// <summary>
        /// This constructor is for deserialization / ORM purposes.
        /// </summary>
        private Skill()
        {
        }

        /// <summary>
        /// Creates a new instance of the Skill class with the specified ID, name, parent skill ID (if any), and skill group ID.
        /// </summary>
        /// <param name="id">The ID of the skill.</param>
        /// <param name="name">The name of the skill.</param>
        /// <param name="parentSkillId">The ID of the parent skill, if any.</param>
        /// <param name="skillGroupId">The ID of the skill group to which this skill belongs.</param>
        internal Skill(Guid id, [NotNull] string name, Guid? parentSkillId, Guid skillGroupId)
            : base(id)
        {
            SetName(name);
            ParentSkillId = parentSkillId;
            SkillGroupId = skillGroupId;
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
        /// Changes the parent skill of the skill.
        /// </summary>
        /// <param name="parentSkillId">The ID of the new parent skill, or null if there is no parent skill.</param>
        /// <returns>The skill object.</returns>
        internal Skill ChangeParentSkill(Guid? parentSkillId)
        {
            ParentSkillId = parentSkillId;
            return this;
        }


        /// <summary>
        /// Changes the Skill's SkillGroup by assigning a new SkillGroupId.
        /// </summary>
        /// <param name="skillGroupId">The new SkillGroup Id</param>
        /// <returns>The Skill with the updated SkillGroup.</returns>
        internal Skill ChangeSkillGroup(Guid skillGroupId)
        {
            SkillGroupId = skillGroupId;
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
                maxLength: SkillConsts.MaxNameLength
            );
        }
    }