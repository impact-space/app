using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace ImpactSpace.Core.Projects
{
    public class ProjectCategory : Entity<Guid>, IMultiTenant
    {
        public string Name { get; private set; }
        public List<Project> Projects { get; private set; } = new();
        public Guid? TenantId { get; set; }

        protected ProjectCategory()
        {
            // This constructor is for deserialization / ORM purposes
        }

        internal ProjectCategory(Guid id, [NotNull] string name)
            : base(id)
        {
            SetName(name);
        }

        internal ProjectCategory ChangeName([NotNull] string name)
        {
            SetName(name);
            return this;
        }

        private void SetName([NotNull] string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: ProjectCategoryConsts.MaxNameLength
            );
        }
    }
}