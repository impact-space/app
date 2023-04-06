using System;
using ImpactSpace.Core.Common;
using ImpactSpace.Core.Skills;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace ImpactSpace.Core.Projects;

/// <summary>
    /// Represents a ProjectSkill entity that associates a Skill with a Project and its proficiency level.
    /// </summary>
    public class ProjectSkill : Entity, IMultiTenant, IHasConcurrencyStamp
    {
        /// <summary>
        /// Gets the associated Project's Id.
        /// </summary>
        public Guid ProjectId { get; private set; }

        /// <summary>
        /// Gets the associated Project.
        /// </summary>
        public virtual Project Project { get; private set; }

        /// <summary>
        /// Gets the associated Skill's Id.
        /// </summary>
        public Guid SkillId { get; private set; }

        /// <summary>
        /// Gets the associated Skill.
        /// </summary>
        public virtual Skill Skill { get; private set; }

        /// <summary>
        /// Gets the proficiency level for the skill in the project.
        /// </summary>
        public ProficiencyLevel ProficiencyLevel { get; private set; }

        /// <summary>
        /// Gets the Tenant Id for multi-tenancy support.
        /// </summary>
        public Guid? TenantId { get; private set; }
        
        public string ConcurrencyStamp { get; set; }

        /// <summary>
        /// Private constructor for deserialization / ORM purposes.
        /// </summary>
        protected ProjectSkill()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectSkill"/> class.
        /// </summary>
        /// <param name="projectId">The associated Project's Id.</param>
        /// <param name="skillId">The associated Skill's Id.</param>
        /// <param name="proficiencyLevel">The proficiency level for the skill in the project.</param>
        public ProjectSkill(Guid projectId, Guid skillId, ProficiencyLevel proficiencyLevel)
        {
            ProjectId = projectId;
            SkillId = skillId;
            ProficiencyLevel = proficiencyLevel;
        }

        /// <summary>
        /// Sets the proficiency level for the skill in the project.
        /// </summary>
        /// <param name="proficiencyLevel">The proficiency level to set.</param>
        /// <returns>The updated <see cref="ProjectSkill"/> instance.</returns>
        public ProjectSkill ChangeProficiencyLevel(ProficiencyLevel proficiencyLevel)
        {
            ProficiencyLevel = proficiencyLevel;
            return this;
        }

        public override object[] GetKeys()
        {
            return new object[] { ProjectId, SkillId };
        }
    }